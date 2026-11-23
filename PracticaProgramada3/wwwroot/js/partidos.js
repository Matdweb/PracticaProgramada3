// Pantalla de administración de partidos políticos (CRUD por AJAX contra el PartidosController del MVC)
(() => {
    const Partidos = {
        init() {
            this.$seccion = $('#partidos');
            this.urls = {
                obtener: this.$seccion.data('urlObtener'),
                porId: this.$seccion.data('urlPorId'),
                crear: this.$seccion.data('urlCrear'),
                actualizar: this.$seccion.data('urlActualizar'),
                eliminar: this.$seccion.data('urlEliminar')
            };

            $('#btnNuevoPartido').on('click', () => this.nuevo());
            $('#btnCancelarFormulario').on('click', () => this.cancelar());
            $('#formPartido').on('submit', e => { e.preventDefault(); this.guardar(); });
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

        dibujar(partidos) {
            $('#sinPartidos').toggleClass('d-none', partidos.length > 0);

            const html = partidos.map(p => `
                <tr>
                    <td>${App.escapar(p.nombre)}</td>
                    <td>
                        <div class="people-actions">
                            <button type="button" class="people-action" data-editar="${p.id}" title="Editar">
                                <i class="bi bi-pencil"></i>
                            </button>
                            <button type="button" class="people-action people-action-danger" data-eliminar="${p.id}" title="Eliminar">
                                <i class="bi bi-trash"></i>
                            </button>
                        </div>
                    </td>
                </tr>`).join('');

            $('#listaPartidos').html(html);
            $('#listaPartidos [data-editar]').on('click', e => this.editar(Number($(e.currentTarget).data('editar'))));
            $('#listaPartidos [data-eliminar]').on('click', e => this.eliminar(Number($(e.currentTarget).data('eliminar'))));
        },

        nuevo() {
            $('#tituloFormulario').text('Nuevo partido');
            $('#partidoId').val('0');
            $('#partidoNombre').val('');
            this.ocultarErroresCampos();
            $('#panelFormulario').removeClass('d-none');
            $('#partidoNombre').trigger('focus');
        },

        editar(id) {
            $.getJSON(this.urls.porId, { id })
                .done(respuesta => {
                    if (!respuesta.esCorrecto) {
                        App.aviso('error', 'No se pudo cargar', respuesta.mensaje);
                        return;
                    }

                    $('#tituloFormulario').text('Editar partido');
                    $('#partidoId').val(respuesta.dato.id);
                    $('#partidoNombre').val(respuesta.dato.nombre);
                    this.ocultarErroresCampos();
                    $('#panelFormulario').removeClass('d-none');
                    $('#partidoNombre').trigger('focus');
                })
                .fail(xhr => App.aviso('error', 'Error', App.mensajeDeError(xhr)));
        },

        cancelar() {
            $('#panelFormulario').addClass('d-none');
            this.ocultarErroresCampos();
        },

        guardar() {
            const nombre = $('#partidoNombre').val().trim();
            const id = Number($('#partidoId').val());
            this.ocultarErroresCampos();

            if (nombre.length < 2 || nombre.length > 100) {
                this.errorCampo('#partidoNombre', '#errorNombre', 'El nombre debe tener entre 2 y 100 caracteres.');
                return;
            }

            const datos = { id, nombre };
            const url = id === 0 ? this.urls.crear : this.urls.actualizar;
            const $boton = $('#btnGuardarPartido').prop('disabled', true);

            $.ajax({
                url,
                type: 'POST',
                data: datos,
                headers: { RequestVerificationToken: App.tokenAntiforgery() }
            })
                .done(respuesta => {
                    if (respuesta.esCorrecto) {
                        App.toast(respuesta.mensaje || 'Partido guardado.');
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
                title: '¿Eliminar partido?',
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
                            App.toast(respuesta.mensaje || 'Partido eliminado.');
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
            $('#errorNombre').addClass('d-none').text('');
        },

        error(mensaje) {
            $('#alertaPartidos').text(mensaje).removeClass('d-none');
        },

        ocultarError() {
            $('#alertaPartidos').addClass('d-none').text('');
        }
    };

    $(() => Partidos.init());
})();
