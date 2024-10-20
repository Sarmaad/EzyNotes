import { Button } from 'primereact/button';
import { useCreateNote, useGetNote, useUpdateNote } from '@/services/NoteService';
import { useRef } from 'react';
import { useNavigate } from '@tanstack/react-router';
import {
    MDXEditor,
    MDXEditorMethods,
    headingsPlugin,
    UndoRedo,
    BoldItalicUnderlineToggles,
    quotePlugin,
    toolbarPlugin,
    markdownShortcutPlugin,
    listsPlugin,
    ListsToggle,
    Separator,
} from '@mdxeditor/editor';

import '@mdxeditor/editor/style.css';
import { Panel } from 'primereact/panel';

export default function EditNote({ id }: { id?: string }) {
    const navigate = useNavigate();
    const { data } = useGetNote(id);
    const { mutateAsync: createNoteAsync, isPending: createPending } = useCreateNote();
    const { mutateAsync: updateNoteAsync, isPending: updatePending } = useUpdateNote();
    const editor = useRef<MDXEditorMethods>(null);

    const handleSave = async () => {
        if (editor.current && !id) {
            await createNoteAsync(editor.current.getMarkdown());
        }
        if (editor.current && id) {
            await updateNoteAsync({ id, content: editor.current.getMarkdown() });
        }

        navigate({ to: '/app' });
    };

    return (
        <Panel header={<span>{(id && 'Edit Note') || 'New Note'}</span>}>
            <MDXEditor
                key={data?.content}
                markdown={data?.content || ''}
                contentEditableClassName="prose min-h-80"
                plugins={[
                    headingsPlugin(),
                    quotePlugin(),
                    markdownShortcutPlugin(),
                    listsPlugin(),
                    toolbarPlugin({
                        toolbarContents: () => (
                            <>
                                <UndoRedo />
                                <Separator />
                                <BoldItalicUnderlineToggles />
                                <Separator />
                                <ListsToggle />
                            </>
                        ),
                    }),
                ]}
                ref={editor}
            />
            <div className={`flex flex-wrap align-items-center justify-content-between gap-3`}>
                <div className="flex align-items-center gap-2">
                    <Button icon="pi pi-check" onClick={handleSave} loading={createPending || updatePending}>
                        <span className='pl-2'>Save</span>
                    </Button>
                    <Button
                        label="Cancel"
                        severity="secondary"
                        icon="pi pi-times"
                        className="ml-2"
                        onClick={() => navigate({ to: '/app' })}
                    />
                </div>
            </div>
        </Panel>
    );
}
