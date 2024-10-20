import { RouterType } from '@/main';
import { useAuth0 } from '@auth0/auth0-react';
import { RouterProvider } from '@tanstack/react-router';
import { useEffect, useState } from 'react';

export default function RouteProvider({ router }: { router: RouterType }) {
    const { isLoading, isAuthenticated, loginWithRedirect, user, getAccessTokenSilently } = useAuth0();

    const [isReady, setReady] = useState(false);
    const [token, setToken] = useState<string>();

    useEffect(() => {
        if (!isLoading) {
            if (isAuthenticated) {
                getAccessTokenSilently().then((t) => setToken(t));
            } else {
                setReady(true);
            }
        }
    }, [isLoading]);

    useEffect(() => {
        if (token && !isReady) {
            setReady(true);
        }
    }, [token]);

    return <>{isReady && <RouterProvider router={router} context={{ loginWithRedirect, user, token }} />}</>;
}
