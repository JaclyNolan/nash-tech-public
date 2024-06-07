import { FC, useEffect, useRef, useState } from "react";
import { Helmet } from "react-helmet-async";
import { Box, Button, Container, Modal, Stack, TableCell, Typography } from '@mui/material';
import axiosInstance from "../../../axiosInstance";
import Iconify from "../../../shared/components/iconify/Iconify";
import ListTable from "../../../shared/components/table/ListTable";
import { URLConstants } from "../../../common/constants";
import { HeadLabel } from "../../../shared/components/table/ListHead";
import BookAdd from "./BookAdd";
import BookEdit from "./BookEdit";
import { nameof } from "../../../common/helper";
import { toStandardExtendedFormat } from './../../../common/helper';

const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 552,
    bgcolor: 'background.paper',
    border: '0px solid #000',
    boxShadow: 24,
    p: 4,
};

export interface Book {
    id: string,
    title: string,
    author: string,
    description: string,
    categoryId: string,
    dateCreated: string
}

export interface FetchData<T> {
    data: T[],
    pageNumber: number,
    pageSize: number,
    totalCount: number
}

export interface ListTableUseState<T> {
    openEntryId: string,
    page: number,
    order: "asc" | "desc",
    selected: string[],
    orderBy: string,
    filterName: string,
    rowsPerPage: number,
    fetchData: FetchData<T> | null,
    isFetchingData: boolean,
    editModalOpen: boolean,
    addModalOpen: boolean,
    assignModalOpen: boolean,
}



const BookList:FC = () => {
    const [listTableUseStates, setListTableUseStates] = useState<ListTableUseState<Book>>({
        openEntryId: "",
        page: 0,
        order: "asc",
        orderBy: "title",
        filterName: '',
        rowsPerPage: 5,
        fetchData: null,
        isFetchingData: false,
        editModalOpen: false,
        addModalOpen: false,
        assignModalOpen: false,
        selected: []
    });

    const placeholderSearchText = 'Search book by title...';
    const fetchRef = useRef<number>(0);

    const TABLE_HEAD: HeadLabel[] = [
        { id: nameof<Book>('id'), label: "Id", alignRight: false, orderable: true },
        { id: nameof<Book>('title'), label: 'Title', alignRight: false, orderable: true },
        { id: nameof<Book>('author'), label: "Author", alignRight: false },
        { id: nameof<Book>('description'), label: 'Description', alignRight: false },
        { id: nameof<Book>('dateCreated'), label: 'Created At', alignRight: false, orderable: true },
        { id: nameof<Book>('categoryId'), label: 'Category Id', alignRight: false },
        { id: 'actions', label: 'Action' },
    ];

    const TABLE_ROW = (book: Book) => (
        <>
            <TableCell align="left">{book.id}</TableCell>
            <TableCell align="left"><Typography variant="subtitle2" noWrap>{book.title}</Typography></TableCell>
            <TableCell align="left">{book.author}</TableCell>
            <TableCell align="left">{book.description}</TableCell>
            <TableCell align="left">{toStandardExtendedFormat(book.dateCreated)}</TableCell>
            <TableCell align="left">{book.categoryId}</TableCell>
        </>
    );

    const setFetchingData = (isFetching: boolean) => {
        setListTableUseStates((prevState) => ({ ...prevState, isFetchingData: isFetching }));
    };

    const setFetchData = (data: FetchData<Book>) => {
        setListTableUseStates((prevState) => ({ ...prevState, fetchData: data }));
    };

    const fetchBookData = async () => {
        setFetchingData(true);
        const { page, order, orderBy, filterName, rowsPerPage } = listTableUseStates;
        const pagePlusOne = page + 1;
        const params = {
            page: pagePlusOne,
            sortField: orderBy,
            sortOrder: order,
            search: filterName,
            perPage: rowsPerPage,
        };
        fetchRef.current += 1;
        const fetchId = fetchRef.current;
        const response = await axiosInstance.get(URLConstants.BOOK.GETALL, { params });
        if (fetchId !== fetchRef.current) return;
        setFetchData(response.data);
        setFetchingData(false);
    };

    const handleDeleteBook = async (id: string) => {
        setFetchingData(true);
        const response = await axiosInstance.delete(`${URLConstants.BOOK.DELETE}/${id}`);
        return response;
    };

    const refreshTable = () => {
        if (listTableUseStates.page === 0) fetchBookData();
        else setListTableUseStates((prevState) => ({ ...prevState, page: 0 }));
    };

    const handleAddModalOpen = () => setListTableUseStates((prevState) => ({ ...prevState, addModalOpen: true }));
    const handleAddModalClose = () => setListTableUseStates((prevState) => ({ ...prevState, addModalOpen: false }));
    const handleEditModalClose = () => setListTableUseStates((prevState) => ({ ...prevState, editModalOpen: false }));

    useEffect(() => {
        fetchBookData();
    }, [listTableUseStates.page, listTableUseStates.orderBy, listTableUseStates.order, listTableUseStates.filterName, listTableUseStates.rowsPerPage]);

    return (
        <>
            <Helmet>
                <title> Book Management </title>
            </Helmet>

            <Container>
                <Stack direction="row" alignItems="center" justifyContent="space-between" mb={5}>
                    <Typography variant="h4" gutterBottom>
                        Book Management
                    </Typography>
                    <Button onClick={handleAddModalOpen} variant="contained" startIcon={<Iconify icon="eva:plus-fill" />}>
                        New Book
                    </Button>
                </Stack>
                <ListTable
                    TABLE_HEAD={TABLE_HEAD}
                    TABLE_ROW={TABLE_ROW}
                    listTableUseStates={listTableUseStates}
                    setListTableUseStates={setListTableUseStates}
                    searchText={placeholderSearchText}
                    handleDeleteEntry={handleDeleteBook}
                    refreshTable={refreshTable}
                />
            </Container>
            <Modal
                key='add'
                open={listTableUseStates.addModalOpen}
                onClose={handleAddModalClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <BookAdd fetchList={fetchBookData} />
                </Box>
            </Modal>

            <Modal
                key='edit'
                open={listTableUseStates.editModalOpen}
                onClose={handleEditModalClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <BookEdit fetchList={fetchBookData} entryId={listTableUseStates.openEntryId} />
                </Box>
            </Modal>
        </>
    );
}

export default BookList