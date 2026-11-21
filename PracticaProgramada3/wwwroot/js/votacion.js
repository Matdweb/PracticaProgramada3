// Pantalla de votación: cédula -> elegir partido -> voto registrado.
// Todo el código de esta pantalla vive aquí, a propósito (no depende de funciones compartidas de crear/enviar).
(() => {
    const Votacion = {
        cedula: '',

        init() {
            this.$seccion = $('#votacion');
            this.urls = {
                verificar: this.$seccion.data('urlVerificar'),
                partidos: this.$seccion.data('urlPartidos'),
                votar: this.$seccion.data('urlVotar')
            };

            this.$cedula = $('#cedula');
            this.$cedulaError = $('#cedulaError');

            $('#formCedula').on('submit', e => { e.preventDefault(); this.verificar(); });
            $('#btnVotar').on('click', () => this.confirmarVoto());
            $('#btnCancelar, #btnNuevo').on('click', () => this.reiniciar());
            this.$cedula.on('input', () => this.ocultarError());

            this.$cedula.trigger('focus');
        },

        // Muestra un solo paso a la vez
        mostrarPaso(selector) {
            $('.paso').addClass('d-none');
            $(selector).removeClass('d-none');
        },

        mostrarError(mensaje) {
            this.$cedulaError.text(mensaje).removeClass('d-none');
            this.$cedula.addClass('is-invalid');
        },

        ocultarError() {
            this.$cedulaError.addClass('d-none').text('');
            this.$cedula.removeClass('is-invalid');
        },

        // Paso 1: la cédula debe estar inscrita y no haber votado
        verificar() {
            const cedula = this.$cedula.val().trim();

            if (cedula.replace(/\D/g, '').length !== 9) {
                this.mostrarError('Ingresa una cédula válida (9 dígitos).');
                return;
            }

            const $boton = $('#formCedula [type=submit]').prop('disabled', true);

            $.ajax({
                url: this.urls.verificar,
                type: 'POST',
                data: { cedula },
                headers: { RequestVerificationToken: App.tokenAntiforgery() }
            })
                .done(respuesta => {
                    if (respuesta.esCorrecto) {
                        this.cedula = cedula;
                        this.cargarPartidos(respuesta.dato);
                    } else {
                        this.alertarCedula(respuesta);
                    }
                })
                .fail(xhr => App.aviso('error', 'Error', App.mensajeDeError(xhr)))
                .always(() => $boton.prop('disabled', false));
        },

        // Alerta según el código que devolvió la API
        alertarCedula(respuesta) {
            switch (respuesta.codigo) {
                case 400:
                    this.mostrarError(respuesta.mensaje);
                    break;
                case 404: // la cédula no está inscrita
                    App.aviso('warning', 'Cédula no inscrita', respuesta.mensaje);
                    break;
                case 409: // ya votó
                    App.aviso('info', 'Ya votaste', respuesta.mensaje);
                    break;
                default:
                    App.aviso('error', 'No se pudo verificar', respuesta.mensaje);
            }
        },

        // Paso 2: trae los partidos y los dibuja
        cargarPartidos(votante) {
            $('#nombreVotante').text(votante.nombreCompleto);

            $.getJSON(this.urls.partidos)
                .done(respuesta => {
                    if (!respuesta.esCorrecto || !respuesta.dato.length) {
                        App.aviso('error', 'Sin partidos', respuesta.mensaje || 'No hay partidos disponibles para votar.');
                        return;
                    }

                    this.dibujarPartidos(respuesta.dato);
                    this.mostrarPaso('#pasoPartido');
                })
                .fail(xhr => App.aviso('error', 'Error', App.mensajeDeError(xhr)));
        },

        dibujarPartidos(partidos) {
            const html = partidos.map(p => `
                <label class="partido">
                    <input type="radio" name="partido" value="${p.id}" data-nombre="${App.escapar(p.nombre)}">
                    <span class="partido-marca">${App.escapar(p.nombre.charAt(0).toUpperCase())}</span>
                    <span class="partido-nombre">${App.escapar(p.nombre)}</span>
                    <i class="bi bi-check-circle-fill partido-check"></i>
                </label>`).join('');

            $('#listaPartidos').html(html);
        },

        // Paso 3: confirmar y registrar el voto
        confirmarVoto() {
            const $elegido = $('input[name=partido]:checked');

            if (!$elegido.length) {
                App.toast('Selecciona un partido para continuar.', 'warning');
                return;
            }

            const partidoId = Number($elegido.val());
            const nombre = String($elegido.data('nombre'));

            Swal.fire({
                title: '¿Confirmas tu voto?',
                text: `Vas a votar por ${nombre}. Esta acción no se puede deshacer.`,
                icon: 'question',
                showCancelButton: true,
                reverseButtons: true,
                confirmButtonText: 'Sí, votar',
                cancelButtonText: 'Revisar',
                confirmButtonColor: '#1b1b1f',
                cancelButtonColor: '#8a8a93'
            }).then(resultado => {
                if (resultado.isConfirmed) this.enviarVoto(partidoId);
            });
        },

        enviarVoto(partidoId) {
            const $boton = $('#btnVotar').prop('disabled', true);

            $.ajax({
                url: this.urls.votar,
                type: 'POST',
                data: { cedula: this.cedula, partidoId },
                headers: { RequestVerificationToken: App.tokenAntiforgery() }
            })
                .done(respuesta => {
                    if (respuesta.esCorrecto) {
                        $('#partidoElegido').text(respuesta.dato.nombre);
                        this.mostrarPaso('#pasoFinal');
                    } else {
                        // Si ya había votado (409) o dejó de estar inscrita, vuelve al inicio
                        App.aviso('error', 'No se pudo registrar el voto', respuesta.mensaje)
                            .then(() => { if (respuesta.codigo !== 500 && respuesta.codigo !== 503) this.reiniciar(); });
                    }
                })
                .fail(xhr => App.aviso('error', 'Error', App.mensajeDeError(xhr)))
                .always(() => $boton.prop('disabled', false));
        },

        // Deja la pantalla lista para la siguiente persona
        reiniciar() {
            this.cedula = '';
            this.$cedula.val('');
            this.ocultarError();
            $('#listaPartidos').empty();
            this.mostrarPaso('#pasoCedula');
            this.$cedula.trigger('focus');
        }
    };

    $(() => Votacion.init());
})();
