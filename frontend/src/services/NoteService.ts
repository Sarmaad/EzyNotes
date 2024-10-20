import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { Queries } from './constants';
import { useRouteContext } from '@tanstack/react-router';
import { SearchResponse, Note } from '@/utils/types';

export function useSearchNotes(page: number = 1, limit: number = 10) {
    const { token } = useRouteContext({ strict: false });
    return useQuery({
        queryKey: [Queries.Notes.Search, page, limit],
        queryFn: async (): Promise<SearchResponse<Note>> => {
            const res = await fetch(`${import.meta.env.VITE_endpoint}/notes?page=${page}&limit=${limit}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (!res.ok) {
                throw Error(res.statusText);
            }

            return await res.json();
        },
    });
}

export function useCreateNote() {
    const { token } = useRouteContext({ strict: false });
    const client = useQueryClient();
    return useMutation({
        mutationFn: async (content: string): Promise<Note> => {
            const res = await fetch(`${import.meta.env.VITE_endpoint}/notes`, {
                method: 'POST',
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ content }),
            });

            if (!res.ok) {
                throw Error(res.statusText);
            }
            return await res.json();
        },
        onSuccess() {
            client.invalidateQueries({ queryKey: [Queries.Notes.Search] });
        },
    });
}

export function useGetNote(noteId?: string) {
    const { token } = useRouteContext({ strict: false });
    return useQuery({
        queryKey: [Queries.Notes.Get, noteId],
        queryFn: async (): Promise<Note | undefined> => {
            if (!noteId) return undefined;

            const res = await fetch(`${import.meta.env.VITE_endpoint}/notes/${noteId}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (!res.ok) {
                throw Error(res.statusText);
            }

            return await res.json();
        },
    });
}

export function useUpdateNote() {
    const { token } = useRouteContext({ strict: false });
    const client = useQueryClient();

    return useMutation({
        mutationFn: async (note: { id: string; content: string }): Promise<Note> => {
            const res = await fetch(`${import.meta.env.VITE_endpoint}/notes/${note.id}`, {
                method: 'PUT',
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ content: note.content }),
            });

            if (!res.ok) {
                throw Error(res.statusText);
            }

            return (await res.json()) as Note;
        },
        onSuccess(data) {
            client.invalidateQueries({ queryKey: [Queries.Notes.Get, data.id] });
            client.invalidateQueries({ queryKey: [Queries.Notes.Search] });
        },
    });
}
