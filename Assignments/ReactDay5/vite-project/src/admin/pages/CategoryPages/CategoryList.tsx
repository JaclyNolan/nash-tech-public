import { FC, useEffect, useRef, useState } from "react";
import { Helmet } from "react-helmet-async";
import { Box, Button, Container, Modal, Stack, TableCell, Typography } from '@mui/material';
import axiosInstance from "../../../axiosInstance";
import Iconify from "../../../shared/components/iconify/Iconify";
import ListTable from "../../../shared/components/table/ListTable";
import { URLConstants } from "../../../common/constants";
import { HeadLabel } from "../../../shared/components/table/ListHead";
import { nameof } from "../../../common/helper";
import { Category } from "../../../admin/pages/BookPages/BookAdd";
import { FetchData, ListTableUseState } from "../../../admin/pages/BookPages/BookList";
import { CategoryAdd, CategoryEdit } from "../CategoryPages"

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

const TABLE_ROW = (category: Category) => (
    <>
        <TableCell align="left">{category.id}</TableCell>
        <TableCell align="left"><Typography variant="subtitle2" noWrap>{category.name}</Typography></TableCell>
        <TableCell align="left">{category.dateCreated}</TableCell>
    </>
);

const CategoryList: FC = () => {
    const [listTableUseStates, setListTableUseStates] = useState<ListTableUseState<Category>>({
        openEntryId: "",
        page: 0,
        order: "asc",
        orderBy: "name",
        filterName: '',
        rowsPerPage: 5,
        fetchData: null,
        isFetchingData: false,
        editModalOpen: false,
        addModalOpen: false,
        assignModalOpen: false,
        selected: []
    });

    const placeholderSearchText = 'Search category by name...';
    const fetchRef = useRef<number>(0);

    const TABLE_HEAD: HeadLabel[] = [
        { id: nameof<Category>('id'), label: "Id", alignRight: false, orderable: true },
        { id: nameof<Category>('name'), label: 'Name', alignRight: false, orderable: true },
        { id: nameof<Category>('dateCreated'), label: 'Created At', alignRight: false, orderable: true },
        { id: 'actions', label: 'Action' },
    ];

    const setFetchingData = (isFetching: boolean) => {
        setListTableUseStates((prevState) => ({ ...prevState, isFetchingData: isFetching }));
    };

    const setFetchData = (data: FetchData<Category>) => {
        setListTableUseStates((prevState) => ({ ...prevState, fetchData: data }));
    };

    const fetchCategoryData = async () => {
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
        const response = await axiosInstance.get(URLConstants.CATEGORY.GETALL, { params });
        if (fetchId !== fetchRef.current) return;
        setFetchData(response.data);
        setFetchingData(false);
    };

    const handleDeleteCategory = async (id: string) => {
        setFetchingData(true);
        const response = await axiosInstance.delete(`${URLConstants.CATEGORY.DELETE}/${id}`);
        return response;
    };

    const refreshTable = () => {
        if (listTableUseStates.page === 0) fetchCategoryData();
        else setListTableUseStates((prevState) => ({ ...prevState, page: 0 }));
    };

    const handleAddModalOpen = () => setListTableUseStates((prevState) => ({ ...prevState, addModalOpen: true }));
    const handleAddModalClose = () => setListTableUseStates((prevState) => ({ ...prevState, addModalOpen: false }));
    const handleEditModalClose = () => setListTableUseStates((prevState) => ({ ...prevState, editModalOpen: false }));

    useEffect(() => {
        fetchCategoryData();
    }, [listTableUseStates.page, listTableUseStates.orderBy, listTableUseStates.order, listTableUseStates.filterName, listTableUseStates.rowsPerPage]);

    return (
        <>
            <Helmet>
                <title> Category Management </title>
            </Helmet>

            <Container>
                <Stack direction="row" alignItems="center" justifyContent="space-between" mb={5}>
                    <Typography variant="h4" gutterBottom>
                        Category Management
                    </Typography>
                    <Button onClick={handleAddModalOpen} variant="contained" startIcon={<Iconify icon="eva:plus-fill" />}>
                        New Category
                    </Button>
                </Stack>
                <ListTable
                    TABLE_HEAD={TABLE_HEAD}
                    TABLE_ROW={TABLE_ROW}
                    listTableUseStates={listTableUseStates}
                    setListTableUseStates={setListTableUseStates}
                    searchText={placeholderSearchText}
                    handleDeleteEntry={handleDeleteCategory}
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
                    <CategoryAdd fetchList={fetchCategoryData} />
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
                    <CategoryEdit fetchList={fetchCategoryData} entryId={listTableUseStates.openEntryId} />
                </Box>
            </Modal>
        </>
    );
}

export default CategoryList;
