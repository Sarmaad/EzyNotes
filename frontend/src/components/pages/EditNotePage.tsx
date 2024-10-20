import { useParams } from '@tanstack/react-router';
import EditNote from '../notes/EditNote';

export default function EditNotePage() {
    const { id } = useParams({ from: '/_app/app/notes/$id/edit' });

    return <EditNote id={id} />;
}
