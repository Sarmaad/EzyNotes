import NewNotePage from '@/components/pages/NewNotePage'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/_app/app/notes/new')({
  component: NewNotePage
})
