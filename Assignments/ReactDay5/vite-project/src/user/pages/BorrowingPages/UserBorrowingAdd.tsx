import { LoadingButton } from '@mui/lab';
import { CircularProgress, Grid, IconButton, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Typography } from '@mui/material';
import { AxiosResponse } from 'axios';
import dayjs from 'dayjs';
import * as React from 'react';
import { useEffect, useState } from 'react';
import { Book, FetchData } from '../../../admin/pages/BookPages/BookList';
import axiosInstance from '../../../axiosInstance';
import { URLConstants } from '../../../common/constants';
import GrayBackdrop from '../../../shared/components/gray-backdrop';
import Iconify from '../../../shared/components/iconify';
import DebounceSelect, { SelectOption } from '../../../shared/components/table/DebounceSelect';
import { BorrowingRequestDetailsUser } from './UserBorrowingList';

interface UserBorrowingAddProps {
    fetchList: () => void;
}

export interface UserBorrowingAddRequestBody {
    requestDetails: {
        bookId: string,
        borrowedDate: string,
        returnedDate: string,
    }[]
}

export default function UserBorrowingAdd({ fetchList }: UserBorrowingAddProps) {
    const [isRetrieving, setRetrieving] = useState(false);
    const [isSubmitting, setSubmitting] = useState(false);
    const [isEditMode, setEditMode] = useState(false);

    const [selectedBooks, _setSelectedBooks] = useState<BorrowingRequestDetailsUser[]>([]);
    const [selectedOption, setSelectedOption] = useState<SelectOption[]>([]);

    const setSelectedBooks = (books: BorrowingRequestDetailsUser[]) => {
        _setSelectedBooks(books);
        setSelectedOption(books.map((book): SelectOption => ({
            value: book.bookId,
            label: ""
        })))
    }

    useEffect(() => {
        setRetrieving(false);
    }, []);

    const fetchBooks = async (search: string): Promise<SelectOption[]> => {
        const params = { search };
        const response: AxiosResponse = await axiosInstance.get(URLConstants.BOOK.GETALL, { params });
        const books: FetchData<Book> = response.data;
        return books.data.map((book) => ({
            value: book.id,
            label: `${book.title} by ${book.author}`,
        }));
    };

    const handleFormSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        setSubmitting(true);
        const payload: UserBorrowingAddRequestBody = {
            requestDetails: selectedBooks.map((book) => ({
                bookId: book.bookId,
                borrowedDate: dayjs(book.borrowedDate).format('YYYY-MM-DD'),
                returnedDate: dayjs(book.returnedDate).format('YYYY-MM-DD'),
            })),
        };
        try {
            await axiosInstance.post(URLConstants.BORROWING_REQUEST.ADD, payload);
            alert('Borrowing request added successfully!');
            fetchList();
        } catch (error) {
            console.error('Error adding borrowing request:', error);
        } finally {
            setSubmitting(false);
        }
    };

    const addBookToData = (newBook: SelectOption | null) => {
        if (!newBook) return;
        if (selectedBooks.length >= 5) {
            alert('You can select a maximum of 5 books.');
            return;
        }
        for (const book of selectedBooks) {
            if (book.bookId === newBook.value) return;
        }
        const newBookDetails: BorrowingRequestDetailsUser = {
            id: '',
            bookId: newBook.value,
            borrowedDate: dayjs().format('YYYY-MM-DD'),
            returnedDate: dayjs().add(1, 'day').format('YYYY-MM-DD'),
            bookBorrowingRequestId: '',
            book: {
                id: newBook.value,
                title: newBook.label,
                author: '',
                description: '',
                categoryId: '',
                category: { id: '', name: '' },
            },
        };
        
        setSelectedBooks([newBookDetails, ...selectedBooks]);
    };

    const handleDelete = (id: string) => {
        const updatedBooks = selectedBooks.filter((book) => book.bookId !== id);
        setSelectedBooks(updatedBooks);
    };

    const handleEdit = () => {
        setEditMode(true);
    };

    const handleView = () => {
        setEditMode(false);
        setSelectedBooks([]);
    };

    return (
        <Stack spacing={3}>
            <Typography variant="h6" gutterBottom>
                Add Borrowing Request
            </Typography>
            {isRetrieving ? (
                <CircularProgress />
            ) : (
                <form onSubmit={handleFormSubmit}>
                    <Grid container spacing={3}>
                        <Grid item xs={12} sm={12}>
                            <div style={{ position: 'relative' }}>
                                <DebounceSelect
                                    id="book"
                                    name="book"
                                    label="Add a new book"
                                    placeholder="Search book by title"
                                    value={null}
                                    onChange={(value) => addBookToData(value)}
                                    fetchOptions={fetchBooks}
                                    disableOptions={selectedOption}
                                />
                                <GrayBackdrop open={!isEditMode} style={{ position: 'absolute' }} />
                            </div>
                        </Grid>
                        <Grid item xs={12} sm={12}>
                            <Typography align="right" variant="overline" display="block" gutterBottom paddingRight={2}>
                                Total Books: {selectedBooks.length}/5
                            </Typography>
                            <div style={{ position: 'relative' }}>
                                <TableContainer sx={{ height: 400 }}>
                                    <Table stickyHeader aria-label="sticky table">
                                        <TableHead>
                                            <TableRow>
                                                <TableCell align="left">Id</TableCell>
                                                <TableCell align="left">Title</TableCell>
                                                <TableCell align="left">Borrowed Date</TableCell>
                                                <TableCell align="left">Returned Date</TableCell>
                                                {isEditMode && <TableCell align="left" width={20}>Action</TableCell>}
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {selectedBooks.map((book) => (
                                                <TableRow hover key={book.bookId}>
                                                    <TableCell align="left">{book.bookId}</TableCell>
                                                    <TableCell align="left">{book.book.title}</TableCell>
                                                    <TableCell align="left">
                                                        <TextField
                                                            type="date"
                                                            value={book.borrowedDate}
                                                            inputProps={{ min: dayjs().format('YYYY-MM-DD') }}
                                                            onChange={(e) => {
                                                                const newBorrowedDate = e.target.value;
                                                                const newReturnedDate = dayjs(book.returnedDate).isBefore(newBorrowedDate)
                                                                    ? dayjs(newBorrowedDate).add(1, 'day').format('YYYY-MM-DD')
                                                                    : book.returnedDate;

                                                                const updatedBooks = selectedBooks.map((b) =>
                                                                    b.bookId === book.bookId
                                                                        ? { ...b, borrowedDate: newBorrowedDate, returnedDate: newReturnedDate }
                                                                        : b
                                                                );

                                                                setSelectedBooks(updatedBooks);
                                                            }}
                                                        />
                                                    </TableCell>
                                                    <TableCell align="left">
                                                        <TextField
                                                            type="date"
                                                            value={book.returnedDate}
                                                            inputProps={{
                                                                min: dayjs(book.borrowedDate).add(1, 'day').format('YYYY-MM-DD'),
                                                            }}
                                                            onChange={(e) => {
                                                                const updatedBooks = selectedBooks.map((b) =>
                                                                    b.bookId === book.bookId ? { ...b, returnedDate: e.target.value } : b
                                                                );
                                                                setSelectedBooks(updatedBooks);
                                                            }}
                                                        />
                                                    </TableCell>
                                                    {isEditMode && (
                                                        <TableCell align="left">
                                                            <IconButton onClick={() => handleDelete(book.bookId)}>
                                                                <Iconify icon="eva:trash-2-fill" />
                                                            </IconButton>
                                                        </TableCell>
                                                    )}
                                                </TableRow>
                                            ))}
                                        </TableBody>
                                    </Table>
                                </TableContainer>
                                <GrayBackdrop open={!isEditMode} style={{ position: 'absolute' }} />
                            </div>
                        </Grid>
                        {!isEditMode ? (
                            <Grid item sm={6} xs={6}>
                                <LoadingButton id="edit" onClick={handleEdit} fullWidth size="large" variant="contained">
                                    Edit
                                </LoadingButton>
                            </Grid>
                        ) : (
                            <>
                                <Grid item sm={6} xs={6}>
                                    <LoadingButton id="save" type="submit" loading={isSubmitting} fullWidth size="large" variant="contained">
                                        Save
                                    </LoadingButton>
                                </Grid>
                                <Grid item sm={6} xs={6}>
                                    <LoadingButton id="cancel" onClick={handleView} loading={isSubmitting} color="error" fullWidth size="large" variant="contained">
                                        Cancel
                                    </LoadingButton>
                                </Grid>
                            </>
                        )}
                    </Grid>
                </form>
            )}
        </Stack>
    );
}
