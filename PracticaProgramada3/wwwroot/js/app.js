// Utilidades de INTERFAZ compartidas por todas las pantallas (mensajes y formato).
// La lógica de cada pantalla (votar, resultados) vive en su propio archivo.
window.App = {

    // Evita que un texto con HTML se ejecute al insertarlo en la página (anti-XSS)
    escapar(texto) {
        return String(texto ?? '').replace(/[&<>"']/g, c =>
            ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
    },

    toast(mensaje, icono = 'success') {
        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: icono,
            title: mensaje,
            showConfirmButton: false,
            timer: 2800,
            timerProgressBar: true
        });
    },

    // Alerta de una sola acción (aceptar). Usa "text", así que el mensaje no se interpreta como HTML
    aviso(icono, titulo, texto) {
        return Swal.fire({ icon: icono, title: titulo, text: texto, confirmButtonText: 'Entendido', confirmButtonColor: '#1b1b1f' });
    },

    mensajeDeError(xhr) {
        if (xhr.status === 400) return 'Los datos enviados no son válidos.';
        return 'Ocurrió un error inesperado. Intenta de nuevo.';
    },

    tokenAntiforgery() {
        return $('input[name=__RequestVerificationToken]').first().val();
    }
};
