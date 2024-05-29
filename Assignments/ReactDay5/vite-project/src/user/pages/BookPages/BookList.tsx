import { useEffect, useRef, useState } from "react";
import { Helmet } from "react-helmet-async";
import { Box, Button, Container, Modal, Stack, TableCell, Typography } from '@mui/material';
import axiosInstance from "../../../axiosInstance";
import Iconify from "../../../shared/components/iconify/Iconify";
import ListTable from "../../../shared/components/table/ListTable";
import { URLConstants } from "../../../common/constants";

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

const assignStyle = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 660,
    bgcolor: 'background.paper',
    border: '0px solid #000',
    boxShadow: 24,
    p: 4,
};

export interface Book {
    id: string,
    name: string,
    description: string
}

interface FetchData {
    data: Book[],
    pageNumber: number,
    pageSize: number,
    totalCount: number
}



export default function BookList() {
    const [addModalOpen, setAddModalOpen] = useState<boolean>(false);
    const [editModalOpen, setEditModalOpen] = useState<boolean>(false);
    const [assignModalOpen, setAssignModalOpen] = useState<boolean>(false);

    const [openEntry, setOpenEntry] = useState<Topic | null>(null);

    const [page, setPage] = useState<number>(0);
    const [order, setOrder] = useState<'asc' | 'desc'>('asc');
    const [selected, setSelected] = useState<number[]>([]);
    const [orderBy, setOrderBy] = useState<string>('name');
    const [filterName, setFilterName] = useState<string>('');
    const [rowsPerPage, setRowsPerPage] = useState<number>(5);
    const [fetchData, setFetchData] = useState<FetchData | null>(null);
    const [isFetchingData, setFetchingData] = useState<boolean>(true);

    const searchText = 'Search topic by name...';
    const fetchRef = useRef<number>(0);

    const useStateData = {
        page, setPage,
        order, setOrder,
        selected, setSelected,
        orderBy, setOrderBy,
        filterName, setFilterName,
        rowsPerPage, setRowsPerPage,
        isFetchingData, setFetchingData,
        openEntry, setOpenEntry,
        editModalOpen, setEditModalOpen,
        fetchData, setFetchData
    };

    const TABLE_HEAD = [
        { id: 'id', label: "Id", alignRight: false, orderable: true },
        { id: 'name', label: 'Name', alignRight: false, orderable: true },
        { id: 'description', label: 'Description', alignRight: false },
        { id: 'created_at', label: 'Created At', alignRight: false, orderable: true },
        { id: 'assign' },
        { id: 'actions' },
    ];

    const TABLE_ROW = (topic: Topic) => (
        <>
            <TableCell align="left">{topic.id}</TableCell>
            <TableCell align="left"><Typography variant="subtitle2" noWrap>{topic.name}</Typography></TableCell>
            <TableCell align="left">{topic.description}</TableCell>
            <TableCell align="left">{topic.created_at}</TableCell>
            <TableCell align="left">
                <Button variant='outlined' onClick={() => handleAssignModalOpen(topic)}>Trainers</Button>
            </TableCell>
        </>
    );

    const fetchTopicData = async () => {
        setFetchingData(true);
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
        const response = await axiosInstance.get(URLConstants.STAFF_TOPIC_INDEX_ENDPOINT, { params });
        if (fetchId !== fetchRef.current) return;
        setFetchData(response.data);
        setFetchingData(false);
    };

    const deleteTopic = async (id: number) => {
        setFetchingData(true);
        const response = await axiosInstance.delete(URLConstants.STAFF_TOPIC_DELETE_ENDPOINT.concat(`/${id}`));
        return response;
    };

    const refreshTable = () => {
        if (page === 0) fetchTopicData();
        else setPage(0);
    };

    const handleAddModalOpen = () => setAddModalOpen(true);
    const handleAddModalClose = () => setAddModalOpen(false);
    const handleEditModalClose = () => setEditModalOpen(false);

    const handleAssignModalOpen = (entry: Topic) => {
        setOpenEntry(entry);
        setAssignModalOpen(true);
    };
    const handleAssignModalClose = () => setAssignModalOpen(false);

    useEffect(() => {
        fetchTopicData();
    }, [page, orderBy, order, filterName, rowsPerPage]);

    return (
        <>
            <Helmet>
                <title> Topic Management </title>
            </Helmet>

            <Container>
                <Stack direction="row" alignItems="center" justifyContent="space-between" mb={5}>
                    <Typography variant="h4" gutterBottom>
                        Topic Management
                    </Typography>
                    <Button onClick={handleAddModalOpen} variant="contained" startIcon={<Iconify icon="eva:plus-fill" />}>
                        New Topic
                    </Button>
                </Stack>
                <ListTable
                    TABLE_HEAD={TABLE_HEAD}
                    TABLE_ROW={TABLE_ROW}
                    useStateData={useStateData}
                    searchText={searchText}
                    deleteEntry={deleteTopic}
                    refreshTable={refreshTable}
                />
            </Container>
            <Modal
                key='add'
                open={addModalOpen}
                onClose={handleAddModalClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <TopicAdd fetchList={fetchTopicData} />
                </Box>
            </Modal>

            <Modal
                key='edit'
                open={editModalOpen}
                onClose={handleEditModalClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <TopicEdit fetchList={fetchTopicData} entry={openEntry} />
                </Box>
            </Modal>

            <Modal
                key='assign'
                open={assignModalOpen}
                onClose={handleAssignModalClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={assignStyle}>
                    <TopicTrainerAssign fetchList={fetchTopicData} entry={openEntry} />
                </Box>
            </Modal>
        </>
    );
}
