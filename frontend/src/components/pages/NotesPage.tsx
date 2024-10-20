import { Card } from 'primereact/card';
import { DataView } from 'primereact/dataview';
import { DisplayNote } from '../notes/DisplayNote';

import { useSearchNotes } from '@/services/NoteService';
import { SpeedDial } from 'primereact/speeddial';
import { useNavigate } from '@tanstack/react-router';

import { Note } from '@/utils/types';

export default function NotesPage() {
    const { data } = useSearchNotes();
    const navigate = useNavigate();

    const gridItem = (note: Note) => {
        return (
                <DisplayNote {...note} />
            // <div className="" key={note.id}>
            // </div>
        );
    };

    const itemTemplate = (note: any, layout?: 'list' | 'grid' | (string & Record<string, unknown>)) => {
        if (!note) {
            return;
        } else if (layout === 'grid') return gridItem(note);
    };

    const items = [
        {
            label: 'Add',
            icon: 'pi pi-pencil',
            command: () => {
                navigate({ to: '/app/notes/new' });
            },
        },
    ];

    return (
        <Card title="Notes">
            <div className="flex flex-row-reverse">
                <SpeedDial
                    className="hidden sm:block"
                    model={items}
                    radius={60}
                    type="linear"
                    direction="down"
                    style={{ marginTop: '-4.5rem' }}
                    //style={{ top: 'calc(20% - 2rem)',  }}
                />
            </div>
            <SpeedDial
                className="sm:hidden"
                model={items}
                radius={70}
                type="linear"
                direction="up"
                style={{ top: 'calc(75%)', right: '1rem' }}
            />

            <DataView
                value={data?.results}
                layout={'grid'}
                itemTemplate={itemTemplate}
                pt={{
                    grid: {
                        className: 'grid-cols-1 md:grid-cols-2 gap-4',
                    },
                }}
            />
        </Card>
    );
}
