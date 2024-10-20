import { Note } from '@/utils/types';
import { Link } from '@tanstack/react-router';
import { Card } from 'primereact/card';
import Markdown from 'react-markdown';

export function DisplayNote({ content, id }: Note) {
    return (
        <Card className="rounded-lg h-40 overflow-auto border px-5 py-2" >
            <Link to="/app/notes/$id/edit" params={{ id }}>
                <Markdown className='prose'>{content}</Markdown>
            </Link>
        </Card>
    );
}
