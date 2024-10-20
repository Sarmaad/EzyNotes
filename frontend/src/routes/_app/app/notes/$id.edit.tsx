import EditNotePage from '@/components/pages/EditNotePage'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/_app/app/notes/$id/edit')({
  component: EditNotePage
})
