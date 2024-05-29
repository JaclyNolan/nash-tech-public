import { debounce, filter } from 'lodash';
import { useEffect, useMemo, useState } from 'react';
// @antd
import { Spin } from 'antd';
// @mui
import {
    Card,
    Table,
    Stack,
    Paper,
    Avatar,
    Button,
    Popover,
    Checkbox,
    TableRow,
    MenuItem,
    TableBody,
    TableCell,
    Container,
    Typography,
    IconButton,
    TableContainer,
    TablePagination,
} from '@mui/material';
// components
import Iconify from '../iconify/Iconify';
import Scrollbar from '../scrollbar/Scrollbar';
import ListToolbar from './ListToolbar';
import ListHead from './ListHead';
// sections
// ----------------------------------------------------------------------

interface FetchData {
    current_page: number;
    data: any[];
    first_page_url: string;
    from: number;
    last_page: number;
    last_page_url: string;
    links: { url: string | null; label: string; active: boolean }[];
    next_page_url: string | null;
    path: string;
    per_page: number;
    prev_page_url: string | null;
    to: number;
    total: number;
}

interface StateData {
    page: number;
    setPage: React.Dispatch<React.SetStateAction<number>>;
    order: 'asc' | 'desc';
    setOrder: React.Dispatch<React.SetStateAction<'asc' | 'desc'>>;
    selected: any[];
    setSelected: React.Dispatch<React.SetStateAction<any[]>>;
    orderBy: string;
    setOrderBy: React.Dispatch<React.SetStateAction<string>>;
    filterName: string;
    setFilterName: React.Dispatch<React.SetStateAction<string>>;
    rowsPerPage: number;
    setRowsPerPage: React.Dispatch<React.SetStateAction<number>>;
    isFetchingData: boolean;
    setFetchingData: React.Dispatch<React.SetStateAction<boolean>>;
    openEntry: any;
    setOpenEntry: React.Dispatch<React.SetStateAction<any>>;
    editModalOpen: boolean;
    setEditModalOpen: React.Dispatch<React.SetStateAction<boolean>>;
    fetchData: FetchData | null;
    setFetchData: React.Dispatch<React.SetStateAction<FetchData | null>>;
}

interface ListTableProps {
    TABLE_HEAD: any[];
    TABLE_ROW: (row: any) => JSX.Element;
    searchText: string;
    useStateData: StateData;
    deleteEntry: (id: number) => Promise<any>;
    refreshTable: () => void;
}

export default function ListTable({
    TABLE_HEAD,
    TABLE_ROW,
    searchText,
    useStateData,
    deleteEntry,
    refreshTable
}: ListTableProps) {
    const {
        page,
        setPage,
        order,
        setOrder,
        selected,
        setSelected,
        orderBy,
        setOrderBy,
        filterName,
        setFilterName,
        rowsPerPage,
        setRowsPerPage,
        isFetchingData,
        setFetchingData,
        openEntry,
        setOpenEntry,
        editModalOpen,
        setEditModalOpen,
        fetchData,
        setFetchData
    } = useStateData;

    const [open, setOpen] = useState<HTMLElement | null>(null);

    const fetchDataTotal = fetchData ? fetchData.total : 0;

    const fetchDataLength = fetchData ? fetchData.data.length : 0;

    const [fetchDataData, setFetchDataData] = useState<any[]>(fetchData ? fetchData.data : []);

    useEffect(() => {
        const newFetchDataData = fetchData ? fetchData.data : [];
        setFetchDataData(newFetchDataData);
    }, [fetchData]);

    const handleOpenMenu = (event: React.MouseEvent<HTMLElement>, id: any) => {
        setOpen(event.currentTarget);
        setOpenEntry(id);
    };

    const handleCloseMenu = () => {
        setOpen(null);
        setOpenEntry(null);
    };

    const handleSingleDelete = async () => {
        setOpen(null);
        setOpenEntry(null);
        const response = await deleteEntry(openEntry.id);
        selected.splice(selected.indexOf(openEntry.id), 1);
        refreshTable();
        alert(response.data);
    };

    const handleMultpleDelete = () => {
        let successfulDelete = 0;
        const selectedLen = selected.length;
        const deleteMultipleEntries = async (selectedId: number) => {
            console.log("Deleting: " + selectedId);
            await deleteEntry(selectedId);
            successfulDelete += 1;

            // Remove the entry from the selected array
            const index = selected.indexOf(selectedId);
            if (index !== -1) {
                selected.splice(index, 1);
                const newSelected = selected;
                setSelected(newSelected);
            }

            // Remove the entry from the table data
            const indexToRemove = fetchDataData.findIndex((element) => element.id === selectedId);
            if (indexToRemove !== -1) {
                fetchDataData.splice(indexToRemove, 1);
                setFetchDataData([...fetchDataData]);
            }

            if (successfulDelete === selectedLen) {
                refreshTable();
                alert("All selected deleted successfully!");
            }
        };

        for (let i = selected.length - 1; i >= 0; i--) {
            const selectedId = selected[i];
            deleteMultipleEntries(selectedId);
        }
    };

    const handleRequestSort = (event: React.MouseEvent<unknown>, property: any) => {
        if (property.orderable) {
            const isAsc = orderBy === property.id && order === 'asc';
            setOrder(isAsc ? 'desc' : 'asc');
            setOrderBy(property.id);
        }
    };

    const handleSelectAllClick = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.checked) {
            const newSelecteds = fetchDataData.map((n) => n.id);
            setSelected(newSelecteds);
            return;
        }
        setSelected([]);
    };

    const handleClick = (id: any) => {
        const selectedIndex = selected.indexOf(id);
        let newSelected: any[] = [];
        if (selectedIndex === -1) {
            newSelected = newSelected.concat(selected, id);
        } else if (selectedIndex === 0) {
            newSelected = newSelected.concat(selected.slice(1));
        } else if (selectedIndex === selected.length - 1) {
            newSelected = newSelected.concat(selected.slice(0, -1));
        } else if (selectedIndex > 0) {
            newSelected = newSelected.concat(selected.slice(0, selectedIndex), selected.slice(selectedIndex + 1));
        }
        setSelected(newSelected);
    };

    const handleChangePage = (event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setPage(0);
        setRowsPerPage(parseInt(event.target.value, 10));
    };

    const debounceSetter = useMemo(() => {
        const handleFilterByName = (event: React.ChangeEvent<HTMLInputElement>) => {
            setPage(0);
            setFilterName(event.target.value);
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
        return debounce(handleFilterByName, 700);
    }, []);

    // const emptyRows = page > 0 ? Math.max(0, (1 + page) * rowsPerPage - fetchDataTotal) : 0;

    const isNotFound = !fetchDataTotal && !!filterName;

    return (
        <>
            <Card>
                <ListToolbar
                    numSelected={selected.length}
                    filterName={filterName}
                    onFilterName={debounceSetter}
                    searchText={searchText}
                    // handleMultipleDelte={handleMultpleDelete}
                />
                <Spin spinning={isFetchingData}>
                    <Scrollbar>
                        <TableContainer sx={{ minWidth: 800 }}>
                            <Table>
                                <ListHead
                                    order={order}
                                    orderBy={orderBy}
                                    headLabel={TABLE_HEAD}
                                    rowCount={fetchDataLength}
                                    numSelected={selected.length}
                                    onRequestSort={handleRequestSort}
                                    onSelectAllClick={handleSelectAllClick}
                                />
                                <TableBody>
                                    {fetchDataData.map((row) => {
                                        const { id } = row;
                                        const selectedUser = selected.indexOf(id) !== -1;

                                        return (
                                            <TableRow hover key={id} tabIndex={-1} role="checkbox" selected={selectedUser}>
                                                <TableCell padding="checkbox">
                                                    <Checkbox checked={selectedUser} onChange={() => handleClick(id)} />
                                                </TableCell>

                                                {TABLE_ROW(row)}

                                                <TableCell align="right">
                                                    <IconButton
                                                        size="large"
                                                        color="inherit"
                                                        onClick={(event) => handleOpenMenu(event, row)}
                                                    >
                                                        <Iconify icon={'eva:more-vertical-fill'} />
                                                    </IconButton>
                                                </TableCell>
                                            </TableRow>
                                        );
                                    })}
                                </TableBody>

                                {isNotFound && (
                                    <TableBody>
                                        <TableRow>
                                            <TableCell align="center" colSpan={6} sx={{ py: 3 }}>
                                                <Paper
                                                    sx={{
                                                        textAlign: 'center',
                                                    }}
                                                >
                                                    <Typography variant="h6" paragraph>
                                                        Not found
                                                    </Typography>

                                                    <Typography variant="body2">
                                                        No results found for &nbsp;
                                                        <strong>&quot;{filterName}&quot;</strong>.
                                                        <br /> Try checking for typos or using complete words.
                                                    </Typography>
                                                </Paper>
                                            </TableCell>
                                        </TableRow>
                                    </TableBody>
                                )}
                            </Table>
                        </TableContainer>
                    </Scrollbar>
                </Spin>

                <TablePagination
                    rowsPerPageOptions={[5, 10, 25]}
                    component="div"
                    count={fetchDataTotal}
                    rowsPerPage={rowsPerPage}
                    page={page}
                    onPageChange={handleChangePage}
                    onRowsPerPageChange={handleChangeRowsPerPage}
                />
            </Card>
            <Popover
                open={Boolean(open)}
                anchorEl={open}
                onClose={handleCloseMenu}
                anchorOrigin={{ vertical: 'top', horizontal: 'left' }}
                transformOrigin={{ vertical: 'top', horizontal: 'right' }}
                PaperProps={{
                    sx: {
                        p: 1,
                        width: 140,
                        '& .MuiMenuItem-root': {
                            px: 1,
                            typography: 'body2',
                            borderRadius: 0.75,
                        },
                    },
                }}
            >
                <MenuItem onClick={() => { setEditModalOpen(true) }}>
                    <Iconify icon={'eva:edit-fill'} sx={{ mr: 2 }} />
                    Edit
                </MenuItem>

                <MenuItem sx={{ color: 'error.main' }} onClick={handleSingleDelete}>
                    <Iconify icon={'eva:trash-2-outline'} sx={{ mr: 2 }} />
                    Delete
                </MenuItem>
            </Popover>
        </>
    );
}

