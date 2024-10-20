import { ReactNode } from 'react';
import { Auth0Provider } from '@auth0/auth0-react';
import { RouterType } from '@/main';

export default function AuthProvider({ children, router }: { children: ReactNode; router: RouterType }) {
    return (
        <Auth0Provider
            domain={import.meta.env.VITE_domain}
            clientId={import.meta.env.VITE_client_id}
            authorizationParams={{
                redirect_uri: window.location.origin,
                audience: import.meta.env.VITE_audience,
            }}
            onRedirectCallback={(AppState) => {
                if (AppState?.returnTo) {
                    router.navigate({ to: AppState.returnTo });
                } else {
                    router.navigate({ to: '/app' });
                }
            }}
        >
            {children}
        </Auth0Provider>
    );
}
