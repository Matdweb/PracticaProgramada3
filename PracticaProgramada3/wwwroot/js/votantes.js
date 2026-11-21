(() => {
    const Votantes = {
        init() {
            this.$seccion = $('#votantes');
            this.urls = {
                obtener: this.$seccion.data('urlObtener'),
                porId: this.$seccion.data('urlPorId'),
                crear: this.$seccion.data('urlCrear'),
                actualizar: this.$seccion.data('urlActualizar'),
                eliminar: this.$seccion.data('urlEliminar')
            };

            $('#btnNuevoVotante').on('click', () => this.nuevo());
            $('#btnCancelarFormulario').on('click', () => this.cancelar());
            $('#formVotante').on('submit', e => { e.preventDefault(); this.guardar(); });
            this.cargar();
        },

        cargar() {
            $.getJSON(this.urls.obtener)
                .done(respuesta => {
                    if (!respuesta.esCorrecto) {
                        this.error(respuesta.mensaje);
                        return;
                    }
                    this.ocultarError();
                    this.dibujar(respuesta.dato);
                })
                .fail(xhr => this.error(App.mensajeDeError(xhr)));
        },

        dibujar(votantes) {
            $('#sinVotantes').toggleClass('d-none', votantes.length > 0);

            const html = votantes.map(v => `
                <tr>
                    <td>${App.escapar(v.cedula)}</td>
                    <td>${App.escapar(v.nombreCompleto)}</td>
                    <td>
                        <div class="people-actions">
                            <button type="button" class="people-action" data-editar="${v.id}" title="Editar">
                                <i class="bi bi-pencil"></i>
                            </button>
                            <button type="button" class="people-action people-action-danger" data-eliminar="${v.id}" title="Eliminar">
                                <i class="bi bi-trash"></i>
                            </button>
                        </div>
                    </td>
                </tr>`).join('');

            $('#listaVotantes').html(html);
            $('#listaVotantes [data-editar]').on('click', e => this.editar(Number($(e.currentTarget).data('editar'))));
            $('#listaVotantes [data-eliminar]').on('click', e => this.eliminar(Number($(e.currentTarget).data('eliminar'))));
        },

        nuevo() {
            $('#tituloFormulario').text('Nuevo votante');
            $('#votanteId').val('0');
            $('#votanteCedula').val('');
            $('#votanteNombre').val('');
            this.ocultarErroresCampos();
            $('#panelFormulario').removeClass('d-none');
            $('#votanteCedula').trigger('focus');
        },

        editar(id) {
            $.getJSON(this.urls.porId, { id })
                .done(respuesta => {
                    if (!respuesta.esCorrecto) {
                        App.aviso('error', 'No se pudo cargar', respuesta.mensaje);
                        return;
                    }

                    $('#tituloFormulario').text('Editar votante');
                    $('#votanteId').val(respuesta.dato.id);
                    $('#votanteCedula').val(respuesta.dato.cedula);
                    $('#votanteNombre').val(respuesta.dato.nombreCompleto);
                    this.ocultarErroresCampos();
                    $('#panelFormulario').removeClass('d-none');
                    $('#votanteNombre').trigger('focus');
                })
                .fail(xhr => App.aviso('error', 'Error', App.mensajeDeError(xhr)));
        },

        cancelar() {
            $('#panelFormulario').addClass('d-none');
            this.ocultarErroresCampos();
        },

        guardar() {
            const cedula = $('#votanteCedula').val().trim();
            const nombre = $('#votanteNombre').val().trim();
            const id = Number($('#votanteId').val());
            let valido = true;
            this.ocultarErroresCampos();

            if (!/^\d{9}$/.test(cedula.replace(/\D/g, ''))) {
                this.errorCampo('#votanteCedula', '#errorCedula', 'La cédula debe tener 9 dígitos.');
                valido = false;
            }

            if (nombre.length < 3) {
                this.errorCampo('#votanteNombre', '#errorNombre', 'El nombre completo es obligatorio.');
                valido = false;
            }

            if (!valido) return;

            const datos = { id, cedula, nombreCompleto: nombre };
            const url = id === 0 ? this.urls.crear : this.urls.actualizar;
            const $boton = $('#btnGuardarVotante').prop('disabled', true);

            $.ajax({
                url,
                type: 'POST',
                data: datos,
                headers: { RequestVerificationToken: App.tokenAntiforgery() }
            })
                .done(respuesta => {
                    if (respuesta.esCorrecto) {
                        App.toast(respuesta.mensaje || 'Votante guardado.');
                        this.cancelar();
                        this.cargar();
                    } else {
                        App.aviso(respuesta.codigo === 409 ? 'warning' : 'error', 'No se pudo guardar', respuesta.mensaje);
                    }
                })
                .fail(xhr => App.aviso('error', 'Error', App.mensajeDeError(xhr)))
                .always(() => $boton.prop('disabled', false));
        },

        eliminar(id) {
            Swal.fire({
                title: '¿Eliminar votante?',
                text: 'Esta acción no se puede deshacer.',
                icon: 'warning',
                showCancelButton: true,
                reverseButtons: true,
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar',
                confirmButtonColor: '#d23b3b',
                cancelButtonColor: '#8a8a93'
            }).then(resultado => {
                if (!resultado.isConfirmed) return;

                $.ajax({
                    url: this.urls.eliminar,
                    type: 'POST',
                    data: { id },
                    headers: { RequestVerificationToken: App.tokenAntiforgery() }
                })
                    .done(respuesta => {
                        if (respuesta.esCorrecto) {
                            App.toast(respuesta.mensaje || 'Votante eliminado.');
                            this.cargar();
                        } else {
                            App.aviso(respuesta.codigo === 409 ? 'warning' : 'error', 'No se pudo eliminar', respuesta.mensaje);
                        }
                    })
                    .fail(xhr => App.aviso('error', 'Error', App.mensajeDeError(xhr)));
            });
        },

        errorCampo(selector, selectorError, mensaje) {
            $(selector).addClass('is-invalid');
            $(selectorError).text(mensaje).removeClass('d-none');
        },

        ocultarErroresCampos() {
            $('.people-form-panel .form-control').removeClass('is-invalid');
            $('#errorCedula, #errorNombre').addClass('d-none').text('');
        },

        error(mensaje) {
            $('#alertaVotantes').text(mensaje).removeClass('d-none');
        },

        ocultarError() {
            $('#alertaVotantes').addClass('d-none').text('');
        }
    };

    $(() => Votantes.init());
})();
