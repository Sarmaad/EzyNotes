import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';

import { PrimeReactProvider } from 'primereact/api';
import Tailwind from 'primereact/passthrough/tailwind';

import RouteProvider from '@/providers/RouteProvider';

import 'primeicons/primeicons.css';
import './index.css';
import QueryProvider from './providers/QueryProvider';
import AuthProvider from './providers/AuthProvider';

// Import the generated route tree
import { routeTree } from '@/routeTree.gen';
import { createRouter } from '@tanstack/react-router';

// Create a new router instance
const router = createRouter({
    routeTree,
    context: {
        user: undefined,
        token: undefined,
        loginWithRedirect: undefined!,
    },
});

export type RouterType = typeof router;

// Register the router instance for type safety
declare module '@tanstack/react-router' {
    interface Register {
        router: typeof router;
    }
}

console.log(Tailwind.fieldset);

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <AuthProvider router={router}>
            <QueryProvider>
                <PrimeReactProvider
                    value={{
                        unstyled: false,

                        pt: {
                            ...Tailwind,
                        },
                    }}
                >
                    <RouteProvider router={router} />
                </PrimeReactProvider>
            </QueryProvider>
        </AuthProvider>
    </StrictMode>
);
