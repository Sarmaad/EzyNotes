export type Note = {
    id: string;
    content: string;
    createdOn: Date;
    updatedOn?: Date;
};

export type SearchResponse<T> = {
    results: Array<T>;
    totalRecords: number;
    page: number;
    limit: number;
};
