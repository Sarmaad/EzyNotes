import PublicLayout from '@/components/layouts/PublicLayout';
import { AppState, RedirectLoginOptions, User } from '@auth0/auth0-react';
import { createRootRouteWithContext } from '@tanstack/react-router';

type MyNoteContext = {
    user?: User;
    token?: string;
    loginWithRedirect: (options?: RedirectLoginOptions<AppState> | undefined) => Promise<void>;
};

export const Route = createRootRouteWithContext<MyNoteContext>()({
    component: PublicLayout,
});
