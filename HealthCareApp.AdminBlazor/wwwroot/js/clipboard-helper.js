window.healthAxisClipboard = {
    copyText: async function (text) {
        if (!text) {
            return false;
        }

        try {
            if (navigator.clipboard && window.isSecureContext) {
                await navigator.clipboard.writeText(text);
                return true;
            }
        } catch {
            // Continue to fallback
        }

        try {
            const textArea = document.createElement("textarea");
            textArea.value = text;
            textArea.setAttribute("readonly", "");

            textArea.style.position = "fixed";
            textArea.style.left = "-9999px";
            textArea.style.top = "0";
            textArea.style.opacity = "0";

            document.body.appendChild(textArea);

            textArea.focus();
            textArea.select();
            textArea.setSelectionRange(0, textArea.value.length);

            const copied = document.execCommand("copy");

            document.body.removeChild(textArea);

            return copied;
        } catch {
            return false;
        }
    }
};