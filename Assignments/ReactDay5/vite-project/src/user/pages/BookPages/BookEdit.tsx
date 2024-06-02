import React, { useEffect, useState } from 'react';
import { CircularProgress, Typography, Grid, Stack, TextField } from '@mui/material';
import { LoadingButton } from '@mui/lab';
import { Book } from './BookList';
import DebounceSelect, { SelectOption } from '../../../shared/components/table/DebounceSelect';
import axiosInstance from '../../../axiosInstance';
import { URLConstants } from '../../../common/constants';

interface BookEditProps {
    fetchList: () => void;
    entryId: string;
}

const BookEdit: React.FC<BookEditProps> = ({ fetchList, entryId }) => {
    const [isRetrieving, setRetrieving] = useState(true);
    const [isSubmitting, setSubmitting] = useState(false);

    const [bookId, setBookId] = useState(entryId);
    const [bookName, setBookName] = useState('');
    const [course, setCourse] = useState<SelectOption | null>(null);
    const [initialCourse, setInitialCourse] = useState<SelectOption | null>(null);
    const [bookDescription, setBookDescription] = useState('');

    const fetchBookEditData = async () => {
        const response = await axiosInstance.get(`${URLConstants.BOOK.GETID}/${entryId}`);
        const { name, description, course } = response.data;
        setBookName(name);
        setBookDescription(description);
        const courseOption = { value: course.id, label: `${course.id} ${course.name}` };
        setCourse(courseOption);
        setInitialCourse(courseOption);
        setRetrieving(false);
    };

    const fetchBookAddData = async (search: string): Promise<SelectOption[]> => {
        const params = { search };
        const response = await axiosInstance.get(URLConstants.CATEGORY.GETALL, { params });
        return response.data.course.map((course: { id: number; name: string }) => ({
            value: course.id,
            label: `${course.id} ${course.name}`,
        }));
    };

    const handleFormSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setSubmitting(true);
        const payload = {
            id: entryId,
            name: bookName,
            description: bookDescription,
            course_id: course?.value,
        };
        const response = await axiosInstance.post(`${URLConstants.BOOK.UPDATE}/${entryId}`, payload);
        const book = response.data;
        alert(`Edited successfully ${book.name}`);
        fetchList();
        setSubmitting(false);
    };

    useEffect(() => {
        fetchBookEditData();
    }, []);

    return (
        <Stack spacing={3}>
            <Typography variant="h6" gutterBottom>
                Edit Book
            </Typography>
            {isRetrieving ? (
                <CircularProgress />
            ) : (
                <form onSubmit={handleFormSubmit}>
                    <Grid container spacing={3}>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                required
                                fullWidth
                                disabled
                                id="book_id"
                                name="book_id"
                                label="Book Id"
                                value={bookId}
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                required
                                fullWidth
                                id="book_name"
                                name="book_name"
                                label="Book Name"
                                value={bookName}
                                onChange={(event) => setBookName(event.target.value)}
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <DebounceSelect
                                required
                                id="course_id"
                                name="course_id"
                                label="Course Id"
                                placeholder="Select Course"
                                value={course}
                                onChange={(value) => setCourse(value)}
                                fetchOptions={fetchBookAddData}
                                presetOptions={initialCourse ? [initialCourse] : []}
                            />
                        </Grid>
                        <Grid item xs={12} sm={12}>
                            <TextField
                                id="book_description"
                                name="book_description"
                                label="Description"
                                value={bookDescription}
                                onChange={(event) => setBookDescription(event.target.value)}
                                fullWidth
                                multiline
                                rows={4}
                                placeholder="Description"
                            />
                        </Grid>
                        <Grid item sm={6} xs={6}>
                            <LoadingButton loading={isSubmitting} fullWidth size="large" type="submit" variant="contained">
                                Edit
                            </LoadingButton>
                        </Grid>
                    </Grid>
                </form>
            )}
        </Stack>
    );
};

export default BookEdit;
