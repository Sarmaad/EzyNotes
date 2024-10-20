import { Outlet } from '@tanstack/react-router';
import React, { Suspense } from 'react';

const TanStackRouterDevtools =
    process.env.NODE_ENV === 'production'
        ? () => null // Render nothing in production
        : React.lazy(() =>
              // Lazy load in development
              import('@tanstack/router-devtools').then((res) => ({
                  default: res.TanStackRouterDevtools,
                  // For Embedded Mode
                  // default: res.TanStackRouterDevtoolsPanel
              }))
          );

import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import Header from './header';

export default function PublicLayout() {
    return (
        <div className='container mx-auto'>
            <Header />
            <Outlet />  
            <Suspense>
                <TanStackRouterDevtools />
                <ReactQueryDevtools initialIsOpen={false} />
            </Suspense>
        </div>
    );
}
