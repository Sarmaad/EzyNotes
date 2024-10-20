import { InputSwitch } from 'primereact/inputswitch';
import { MenuItem } from 'primereact/menuitem';
import { useState, useEffect } from 'react';

export function ThemeSwitcherMenuItem(): MenuItem {
    const [isDark, setIsDark] = useState(localStorage.getItem('theme') === 'dark' ?? false);

    const onThemeToggler = () => {
        const root = document.getElementsByTagName('html')[0];

        root.classList.toggle('dark');
        setIsDark(root.classList.contains('dark'));
    };

    useEffect(() => {
        const root = document.getElementsByTagName('html')[0];
        if (isDark && !root.classList.contains('dark')) {
            onThemeToggler();
        }
    }, []);

    useEffect(() => {
        localStorage.setItem('theme', isDark ? 'dark' : 'light');
    }, [isDark]);

    return {
        template: () => {
            return (
                <div className="flex items-center gap-2 ml-4 pb-2">
                    <i className="dark:text-white pi pi-sun" />
                    <InputSwitch checked={isDark} />
                    <i className="dark:text-white pi pi-moon" />
                </div>
            );
        },
        command: onThemeToggler,
    };
}
