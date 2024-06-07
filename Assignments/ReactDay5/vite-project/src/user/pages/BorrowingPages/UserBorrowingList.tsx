import { Box, Button, Card, Container, Modal, Stack, styled, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@mui/material';
import { Spin } from 'antd';
import { AxiosResponse } from 'axios';
import { FC, useEffect, useRef, useState } from "react";
import { Helmet } from "react-helmet-async";
import axiosInstance from "../../../axiosInstance";
import { URLConstants } from "../../../common/constants";
import Iconify from "../../../shared/components/iconify/Iconify";
import Scrollbar from '../../../shared/components/scrollbar/Scrollbar';
import { UserBorrowingTableRow } from '../../components/borrowing';
import UserBorrowingAdd from './UserBorrowingAdd';

const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 900,
    bgcolor: 'background.paper',
    border: '0px solid #000',
    boxShadow: 24,
    p: 4,
};

export interface BorrowingRequestUser {
    id: string,
    requestDate: string,
    status: BookStatus,
    bookBorrowingRequestDetails: BorrowingRequestDetailsUser[]
}

export interface BorrowingRequestDetailsUser {
    id: string,
    borrowedDate: string,
    returnedDate: string,
    bookId: string,
    bookBorrowingRequestId: string,
    book: BookUser
}

export interface BookUser {
    id: string,
    title: string,
    author: string,
    description: string,
    categoryId: string,
    category: CategoryUser,
}

export interface CategoryUser {
    id: string,
    name: string,
}

export enum BookStatus {
    Waiting = 0,
    Rejected = 1,
    Approved = 2
}

const UserBorrowingList: FC = () => {
    const [fetchData, setFetchData] = useState<BorrowingRequestUser[]>([]);
    const [currentMonthRequests, setCurrentMonthRequests] = useState<BorrowingRequestUser[]>([]);
    const [isFetching, setIsFetching] = useState<boolean>(false);
    const [addModalOpen, setAddModalOpen] = useState<boolean>(false);
    const [doShowAll, setDoShowAll] = useState<boolean>(true);

    const fetchBorrowingRequestsData = async () => {
        setIsFetching(true);
        const response: AxiosResponse = await axiosInstance.get(URLConstants.BORROWING_REQUEST.GETALL_USER);
        setFetchData(response.data as BorrowingRequestUser[]);
        setIsFetching(false);
    };

    const fetchCurrentMonthBorrowingRequestsData = async () => {
        setIsFetching(true);
        const response: AxiosResponse = await axiosInstance.get(URLConstants.BORROWING_REQUEST.GET_CURRENT_MONTH);
        setCurrentMonthRequests(response.data as BorrowingRequestUser[]);
        setIsFetching(false);
    };

    useEffect(() => {
        fetchBorrowingRequestsData();
        fetchCurrentMonthBorrowingRequestsData();
    }, [])

    const toggleShowAll = () => {
        setDoShowAll(doShowAll ? false : true);
    }

    const handleAddModalOpen = () => setAddModalOpen(true);
    const handleAddModalClose = () => setAddModalOpen(false);

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
                    <Stack direction="row" alignItems="center" gap={5}>
                        <Button variant="contained" onClick={toggleShowAll}>
                            {doShowAll
                                ? `This month's requests ${currentMonthRequests.length}/3`
                                : `Show All`}
                        </Button>
                        <Button onClick={handleAddModalOpen} variant="contained" startIcon={<Iconify icon="eva:plus-fill" />}>
                            New Borrowing Request
                        </Button>
                    </Stack>
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
                                        {doShowAll && fetchData.map((request) =>
                                            <UserBorrowingTableRow key={request.id} request={request} />)}
                                        {!doShowAll && currentMonthRequests.map((request) =>
                                            <UserBorrowingTableRow key={request.id} request={request} />)}
                                    </TableBody>
                                </Table>
                            </Scrollbar>
                        </TableContainer>
                    </Spin>
                </Card>
            </Container>
            <Modal
                key='add'
                open={addModalOpen}
                onClose={handleAddModalClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <UserBorrowingAdd fetchList={fetchBorrowingRequestsData} />
                </Box>
            </Modal>
        </>
    );
}

export default UserBorrowingList