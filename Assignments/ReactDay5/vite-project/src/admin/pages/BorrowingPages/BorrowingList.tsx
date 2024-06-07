import { Button, Container, Stack, TableCell, Typography } from '@mui/material';
import { FC, useEffect, useRef, useState } from "react";
import { Helmet } from "react-helmet-async";
import axiosInstance from "../../../axiosInstance";
import { URLConstants } from "../../../common/constants";
import Iconify from "../../../shared/components/iconify/Iconify";
import { ListTable } from "../../../shared/components/table";
import { HeadLabel } from "../../../shared/components/table/ListHead";
import { BookStatus } from "../../../user/pages/BorrowingPages/UserBorrowingList";
import { Book, FetchData, ListTableUseState } from "../BookPages/BookList";
import BookStatusComponent from '../../../shared/components/BookStatusComponent';
import { toStandardFormat } from '../../../common/helper';


interface User {
    id: string;
    name: string;
    email: string;
}

interface BorrowingRequest {
    id: string;
    requestDate: string;
    status: BookStatus;
    requestorId: string;
    actionerId: string | null;
    requestor: User;
    actioner: User | null;
    bookBorrowingRequestDetails: BorrowingRequestDetails[];
}

interface BorrowingRequestDetails {
    id: string;
    borrowedDate: string;
    returnedDate: string;
    bookId: string;
    bookBorrowingRequestId: string;
    dateCreated: string;
    book: Book;
}

const BorrowingList: FC = () => {
    const [listTableUseStates, setListTableUseStates] = useState<ListTableUseState<BorrowingRequest>>({
        addModalOpen: false,
        assignModalOpen: false,
        editModalOpen: false,
        openEntryId: '',
        selected: [],
        // Placeholder above
        page: 0,
        order: "asc",
        orderBy: "requestDate",
        filterName: '',
        rowsPerPage: 5,
        fetchData: null,
        isFetchingData: false,
    });

    const placeholderSearchText = 'Search borrowing requests...';
    const fetchRef = useRef<number>(0);

    const TABLE_HEAD: HeadLabel[] = [
        { id: 'requestorName', label: 'Requestor Name', alignRight: false, orderable: true},
        { id: 'actionerName', label: 'Actioner Name', alignRight: false, orderable: true},
        { id: 'requestDate', label: 'Requested Date', alignRight: false, orderable: true },
        { id: 'status', label: 'Status', alignRight: false, orderable: true },
        { id: 'action', label: 'Action'}
    ];

    const TABLE_ROW = (request: BorrowingRequest) => (
        <>
            <TableCell align="left">{request.requestor.name}</TableCell>
            <TableCell align="left">{request.actioner?.name || "N/A"}</TableCell>
            <TableCell align="left">{toStandardFormat(request.requestDate)}</TableCell>
            <TableCell align="left"><BookStatusComponent status={request.status}/></TableCell>
            <TableCell>
                {request.status === BookStatus.Waiting && (
                    <>
                        <Button onClick={() => handleAction(request.id, true)}>Approve</Button>
                        <Button onClick={() => handleAction(request.id, false)}>Reject</Button>
                    </>
                )}
            </TableCell>
        </>
    );

    const handleAction = async (requestId: string, approved: boolean) => {
        try {
            setFetchingData(true)
            const payload = { status: approved ? BookStatus.Approved : BookStatus.Rejected };
    
            await axiosInstance.post(`${URLConstants.BORROWING_REQUEST.UPDATE_STATUS}/${requestId}`, payload);
            fetchBorrowingRequestData();
            alert('Request status updated successfully!');
        } catch (error) {
            console.error('Error updating request status:', error);
        } finally {
            setFetchingData(false)
        }
    };

    const setFetchingData = (isFetching: boolean) => {
        setListTableUseStates((prevState) => ({ ...prevState, isFetchingData: isFetching }));
    };

    const setFetchData = (data: FetchData<BorrowingRequest>) => {
        setListTableUseStates((prevState) => ({ ...prevState, fetchData: data }));
    };

    const fetchBorrowingRequestData = async () => {
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
        const response = await axiosInstance.get(URLConstants.BORROWING_REQUEST.GETALL_ADMIN, { params });
        if (fetchId !== fetchRef.current) return;
        setFetchData(response.data);
        setFetchingData(false);
    };

    useEffect(() => {
        fetchBorrowingRequestData();
    }, [listTableUseStates.page, listTableUseStates.orderBy, listTableUseStates.order, listTableUseStates.filterName, listTableUseStates.rowsPerPage]);

    return (
        <>
            <Helmet>
                <title> Borrowing Requests </title>
            </Helmet>

            <Container>
                <Stack direction="row" alignItems="center" justifyContent="space-between" mb={5}>
                    <Typography variant="h4" gutterBottom>
                        Borrowing Requests
                    </Typography>
                    <Button disabled variant="contained" startIcon={<Iconify icon="eva:plus-fill" />}>
                        New Borrowing
                    </Button>

                </Stack>
                <ListTable
                    TABLE_HEAD={TABLE_HEAD}
                    TABLE_ROW={TABLE_ROW}
                    listTableUseStates={listTableUseStates}
                    setListTableUseStates={setListTableUseStates}
                    searchText={placeholderSearchText}
                    handleDeleteEntry={async () => { }} // Placeholder function for delete
                    refreshTable={fetchBorrowingRequestData}
                    action={false}
                    selectCheckbox={false}
                />
            </Container>
        </>
    );
}

export default BorrowingList;