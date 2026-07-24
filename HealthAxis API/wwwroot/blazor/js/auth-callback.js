window.healthAxisAuth = {
    readAuthPayloadFromHash: function () {
        const hash = window.location.hash || '';

        if (!hash.startsWith('#auth=')) {
            return null;
        }

        try {
            const encodedPayload = hash.substring('#auth='.length);
            const decodedPayload = decodeURIComponent(encodedPayload);
            const json = atob(decodedPayload);
            const payload = JSON.parse(json);

            return {
                AccessToken: payload.accessToken || payload.AccessToken || '',
                RefreshToken: payload.refreshToken || payload.RefreshToken || '',
                Role: payload.role || payload.Role || '',
                Email: payload.email || payload.Email || '',
                UserId: payload.userId || payload.UserId || '',
                ReferenceId: Number(payload.referenceId || payload.ReferenceId || 0)
            };
        } catch (error) {
            console.error('Unable to read auth callback payload.', error);
            return null;
        }
    }
};