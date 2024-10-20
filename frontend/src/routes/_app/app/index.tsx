import NotesPage from '@/components/pages/NotesPage';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/_app/app/')({
    component: NotesPage,
});
