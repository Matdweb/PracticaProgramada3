// Pantalla de resultados: partidos y cantidad de votos de cada uno.
(() => {
    const INTERVALO_ACTUALIZACION_MS = 15000;

    const Resultados = {
        init() {
            this.url = $('#resultados').data('urlObtener');
            this.$lista = $('#listaResultados');

            $('#btnActualizar').on('click', () => this.cargar());

            this.cargar();
            setInterval(() => this.cargar(), INTERVALO_ACTUALIZACION_MS);
        },

        cargar() {
            $.getJSON(this.url)
                .done(respuesta => {
                    if (!respuesta.esCorrecto) {
                        this.mostrarError(respuesta.mensaje);
                        return;
                    }

                    this.ocultarError();
                    this.dibujar(respuesta.dato);
                })
                .fail(xhr => this.mostrarError(App.mensajeDeError(xhr)));
        },

        dibujar(resultados) {
            const total = resultados.reduce((suma, r) => suma + r.cantidadVotos, 0);

            $('#totalVotos').text(total.toLocaleString('es-CR'));
            $('#sinResultados').toggleClass('d-none', resultados.length > 0);

            const html = resultados.map((r, indice) => {
                const porcentaje = total > 0 ? (r.cantidadVotos / total) * 100 : 0;
                const lider = indice === 0 && r.cantidadVotos > 0 ? 'lider' : '';

                return `
                    <div class="result-row ${lider}">
                        <div class="result-info">
                            <span class="result-nombre">${App.escapar(r.nombre)}</span>
                            <span class="result-votos">
                                <strong>${r.cantidadVotos.toLocaleString('es-CR')}</strong>
                                <span class="result-porcentaje">${porcentaje.toFixed(1)}%</span>
                            </span>
                        </div>
                        <div class="result-barra"><div class="result-barra-relleno" style="width:${porcentaje.toFixed(1)}%"></div></div>
                    </div>`;
            }).join('');

            this.$lista.html(html);
            $('#ultimaActualizacion').text('Actualizado a las ' + new Date().toLocaleTimeString('es-CR'));
        },

        mostrarError(mensaje) {
            $('#alertaResultados').text(mensaje).removeClass('d-none');
        },

        ocultarError() {
            $('#alertaResultados').addClass('d-none').text('');
        }
    };

    $(() => Resultados.init());
})();
