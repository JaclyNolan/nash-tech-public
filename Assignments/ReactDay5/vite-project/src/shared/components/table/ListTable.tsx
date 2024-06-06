import { debounce } from 'lodash';
import { FC, useEffect, useMemo, useState } from 'react';
// @antd
import { Spin } from 'antd';
// @mui
import {
    Card,
    Checkbox,
    IconButton,
    MenuItem,
    Paper,
    Popover,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TablePagination,
    TableRow,
    Typography
} from '@mui/material';
// components
import { ListTableUseState } from '../../../admin/pages/BookPages/BookList';
import Iconify from '../iconify/Iconify';
import Scrollbar from '../scrollbar/Scrollbar';
import ListHead, { HeadLabel } from './ListHead';
import ListToolbar from './ListToolbar';
// sections
// ----------------------------------------------------------------------

interface ListTableProps<T> {
    TABLE_HEAD: HeadLabel[];
    TABLE_ROW: (row: any) => JSX.Element;
    searchText: string;
    listTableUseStates: ListTableUseState<T>;
    setListTableUseStates: React.Dispatch<React.SetStateAction<ListTableUseState<T>>>;
    handleDeleteEntry: (id: any) => Promise<any>;
    refreshTable: () => void;
}

const ListTable: FC<ListTableProps<any>> = ({
    TABLE_HEAD,
    TABLE_ROW,
    searchText,
    listTableUseStates,
    setListTableUseStates,
    handleDeleteEntry,
    refreshTable
}) => {
    const {
        page,
        order,
        selected,
        orderBy,
        filterName,
        rowsPerPage,
        isFetchingData,
        openEntryId,
        fetchData,
    } = listTableUseStates;

    const [open, setOpen] = useState<HTMLElement | null>(null);

    const fetchDataTotal = fetchData ? fetchData.totalCount : 0;

    const fetchDataLength = fetchData ? fetchData.data.length : 0;

    const [fetchDataData, setFetchDataData] = useState<any[]>(fetchData ? fetchData.data : []);

    useEffect(() => {
        const newFetchDataData = fetchData ? fetchData.data : [];
        setFetchDataData(newFetchDataData);
    }, [fetchData]);

    const handleOpenMenu = (event: React.MouseEvent<HTMLElement>, id: string) => {
        setOpen(event.currentTarget);
        setListTableUseStates((previous) => ({ ...previous, openEntryId: id }));
    };

    const handleCloseMenu = () => {
        setOpen(null);
        setListTableUseStates((previous) => ({ ...previous, openEntryId: "" }))
    };

    const handleSingleDelete = async () => {
        setOpen(null);
        if (!openEntryId) return;
        await handleDeleteEntry(openEntryId);
        selected.splice(selected.indexOf(openEntryId), 1);
        refreshTable();
        setListTableUseStates((previous) => ({ ...previous, openEntryId: "" }))
        alert(`Delete successfully entry with id: ${openEntryId}`);
    };

    const handleMultpleDelete = () => {
        let successfulDelete = 0;
        const selectedLen = selected.length;
        const deleteMultipleEntries = async (selectedId: string) => {
            console.log("Deleting: " + selectedId);
            await handleDeleteEntry(selectedId);
            successfulDelete += 1;

            // Remove the entry from the selected array
            const index = selected.indexOf(selectedId);
            if (index !== -1) {
                selected.splice(index, 1);
                const newSelected = selected;
                setListTableUseStates((previous) => ({ ...previous, selected: newSelected }))
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

    const handleRequestSort = (_event: React.MouseEvent<unknown>, property: HeadLabel) => {
        if (property.orderable) {
            const isAsc = orderBy === property.id && order === 'asc';
            setListTableUseStates((previous) => ({ ...previous, order: isAsc ? 'desc' : 'asc', orderBy: property.id }))
        }
    };

    const handleSelectAllClick = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.checked) {
            const newSelecteds = fetchDataData.map((n) => n.id);
            setListTableUseStates((previous) => ({ ...previous, selected: newSelecteds }))
            return;
        }
        setListTableUseStates((previous) => ({ ...previous, selected: [] }))
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
        setListTableUseStates((previous) => ({ ...previous, selected: newSelected }))
    };

    const handleChangePage = (_: unknown, newPage: number) => {
        setListTableUseStates((previous) => ({ ...previous, page: newPage }))
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setListTableUseStates((previous) => ({ ...previous, page: 0, rowsPerPage: parseInt(event.target.value, 10) }))
    };

    const debounceSetter = useMemo(() => {
        const handleFilterByName = (event: React.ChangeEvent<HTMLInputElement>) => {
            setListTableUseStates((previous) => ({ ...previous, page: 0, filterName: event.target.value }))
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
        return debounce(handleFilterByName, 700);
    }, []);

    const isNotFound = !fetchDataTotal && !!filterName;

    return (
        <>
            <Card>
                <ListToolbar
                    numSelected={selected.length}
                    onFilterName={debounceSetter}
                    searchText={searchText}
                    handleMultipleDelete={handleMultpleDelete}
                />
                <Spin spinning={isFetchingData}>
                    <TableContainer >
                        <Scrollbar >
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
                                                        onClick={(event) => handleOpenMenu(event, id)}
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
                                                        No results found for &nbsp;<strong>&quot;{filterName}&quot;</strong>.
                                                        <br /> Try checking for typos or using complete words.
                                                    </Typography>
                                                </Paper>
                                            </TableCell>
                                        </TableRow>
                                    </TableBody>
                                )}
                            </Table>
                        </Scrollbar>
                    </TableContainer>
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
                <MenuItem onClick={() => {
                    setListTableUseStates((previous) => ({ ...previous, editModalOpen: true }))
                }}>
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

export default ListTable