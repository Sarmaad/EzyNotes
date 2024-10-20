import { useAuth0 } from '@auth0/auth0-react';
import { useNavigate } from '@tanstack/react-router';
import { Avatar } from 'primereact/avatar';
import { Menubar } from 'primereact/menubar';
import { MenuItem } from 'primereact/menuitem';
import { TieredMenu } from 'primereact/tieredmenu';
import { useRef } from 'react';
import { ThemeSwitcherMenuItem } from './ThemeSwitcherMenuItem';

function Profile() {
    const { logout, isAuthenticated, user, loginWithRedirect } = useAuth0();

    const menu = useRef<TieredMenu | null>(null);
    const items: Array<MenuItem> = [
        {
            icon: 'pi pi-user',
            label: 'Profile',
        },
        ThemeSwitcherMenuItem(),
        {
            separator: true,
        },
        {
            icon: 'pi pi-sign-out',
            label: 'Sign Out',
            command: () => logout(),
        },
    ];

    return (
        <>
            {(isAuthenticated && (
                <>
                    <TieredMenu model={items} popup ref={menu} breakpoint="767px" className="mt-2" />
                    <Avatar
                        image={user?.picture}
                        shape="circle"
                        size="normal"
                        onClick={(e) => menu?.current?.toggle(e)}
                    />
                </>
            )) || <Avatar shape="circle" onClick={() => loginWithRedirect()} />}
        </>
    );
}

export default function Header() {
    const navigate = useNavigate();

    const items: Array<MenuItem> = [
        {
            label: 'Home',
            command: () => navigate({ to: '/' }),
        },
        {
            label: 'My Notes',
            command: () => navigate({ to: '/app' }),
        },
    ];

    return <Menubar model={items} end={Profile} className="mb-4" />;
}
