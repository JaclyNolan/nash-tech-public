import { Button, Card, Container, Stack, styled, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Toolbar, Typography } from '@mui/material';
import { Spin } from 'antd';
import { debounce } from 'lodash';
import { FC, useEffect, useMemo, useRef, useState } from "react";
import { Helmet } from "react-helmet-async";
import axiosInstance from "../../../axiosInstance";
import { URLConstants } from "../../../common/constants";
import Iconify from "../../../shared/components/iconify/Iconify";
import Scrollbar from '../../../shared/components/scrollbar/Scrollbar';
import { UserBorrowingTableRow } from '../../components/borrowing';
import { AxiosResponse } from 'axios';

// const StyledRoot = styled(Toolbar)(({ theme }) => ({
//     height: 96,
//     display: 'flex',
//     justifyContent: 'space-between',
//     padding: theme.spacing(0, 1, 0, 3),
// }));

// const StyledSearch = styled(OutlinedInput)(({ theme }) => ({
//     width: 240,
//     transition: theme.transitions.create(['box-shadow', 'width'], {
//         easing: theme.transitions.easing.easeInOut,
//         duration: theme.transitions.duration.shorter,
//     }),
//     '&.Mui-focused': {
//         width: 320,
//         // boxShadow: theme.customShadows.z8,
//     },
//     '& fieldset': {
//         borderWidth: `1px !important`,
//         borderColor: `${alpha(theme.palette.grey[500], 0.32)} !important`,
//     },
// }));

// const StyleBox = styled(Backdrop)(({ theme }) => ({
//     position: 'absolute',
//     top: '50%',
//     left: '50%',
//     transform: 'translate(-50%, -50%)',
//     width: 552,
//     backgroundColor: theme.palette.background.paper, // Use backgroundColor instead of bgcolor
//     border: '0px solid #000',
//     boxShadow: '10px',
//     p: 4,
// }));

export interface BorrowingRequestUser {
    id: string,
    requestDate: string,
    status: BookStatus,
    bookBorrowingRequestDetails: BorrowingRequestDetailsUser[]
}

interface BorrowingRequestDetailsUser {
    id: string,
    borrowedDate: string,
    returnedDate: string,
    bookId: string,
    bookBorrowingRequestId: string,
    book: BookUser
}

interface BookUser {
    id: string,
    title: string,
    author: string,
    description: string,
    categoryId: string,
    category: CategoryUser,
}

interface CategoryUser {
    id: string,
    name: string,
}

export enum BookStatus {
    Waiting = 0,
    Rejected = 1,
    Approved = 2
}

const UserBorrowingList: FC = () => {
    const [fetchData, setFetchData] = useState<BorrowingRequestUser[] | null>(null);
    const [isFetching, setIsFetching] = useState<boolean>(false);
    const [addModalOpen, setAddModalOpen] = useState<boolean>(false);

    // const placeholderSearchText = 'Search book by title...';
    const fetchRef = useRef<number>(0);

    const fetchBorrowingRequestsData = async () => {
        setIsFetching(true);
        fetchRef.current += 1;
        const fetchId = fetchRef.current;
        const response: AxiosResponse = await axiosInstance.get(URLConstants.BORRROWING_REQUEST.GETALL_USER);
        console.log(response.data)

        if (fetchId !== fetchRef.current) return;
        setFetchData(response.data as BorrowingRequestUser[]);
        setIsFetching(false);
    };

    useEffect(() => {
        fetchBorrowingRequestsData();
    }, [])

    const handleAddModalOpen = () => setAddModalOpen(true);

    return (
        <>
            <Helmet>
                <title> Your Borrowing Requests </title>
            </Helmet>

            <Container>
                <Stack direction="row" alignItems="center" justifyContent="space-between" mb={5}>
                    <Typography variant="h4" gutterBottom>
                        Your Borrowing Requests
                    </Typography>
                    <Button onClick={handleAddModalOpen} variant="contained" startIcon={<Iconify icon="eva:plus-fill" />}>
                        New Borrowing Request
                    </Button>
                </Stack>
                <Card>
                    <Spin spinning={isFetching}>
                        <TableContainer >
                            <Scrollbar >
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell />
                                            <TableCell>Requested Date</TableCell>
                                            <TableCell>Status</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {fetchData && fetchData.map((request) =>
                                            <UserBorrowingTableRow key={request.id} request={request} />)}
                                    </TableBody>
                                </Table>
                            </Scrollbar>
                        </TableContainer>
                    </Spin>
                </Card>
            </Container>
            {/* <Modal
                key='add'
                open={listTableUseStates.addModalOpen}
                onClose={handleAddModalClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <StyledBox>
                    <BookAdd fetchList={fetchBookData} />
                </StyledBox>
            </Modal> */}
        </>
    );
}

export default UserBorrowingList