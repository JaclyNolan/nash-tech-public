// import { Backdrop, Box, Button, Card, Container, Stack, styled, TableCell, Typography } from '@mui/material';
// import { FC, useEffect, useMemo, useRef, useState } from "react";
// import { Helmet } from "react-helmet-async";
// import axiosInstance from "../../../axiosInstance";
// import { URLConstants } from "../../../common/constants";
// import { nameof } from "../../../common/helper";
// import Iconify from "../../../shared/components/iconify/Iconify";
// import { HeadLabel } from "../../../shared/components/table/ListHead";
// import { debounce } from 'lodash';

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

// export interface Book {
//     id: string,
//     title: string,
//     author: string,
//     description: string,
//     categoryId: string,
//     dateCreated: string
// }

// export interface FetchData<T> {
//     data: T[],
//     pageNumber: number,
//     pageSize: number,
//     totalCount: number
// }

// export interface ListTableUseState<T> {
//     openEntryId: string,
//     page: number,
//     order: "asc" | "desc",
//     selected: string[],
//     orderBy: string,
//     filterName: string,
//     rowsPerPage: number,
//     fetchData: FetchData<T> | null,
//     isFetchingData: boolean,
//     editModalOpen: boolean,
//     addModalOpen: boolean,
//     assignModalOpen: boolean,
// }



// const UserBorrowingList: FC = () => {
//     const [listTableUseStates, setListTableUseStates] = useState<ListTableUseState<Book>>({
//         openEntryId: "",
//         page: 0,
//         order: "asc",
//         orderBy: "title",
//         filterName: '',
//         rowsPerPage: 5,
//         fetchData: null,
//         isFetchingData: false,
//         editModalOpen: false,
//         addModalOpen: false,
//         assignModalOpen: false,
//         selected: []
//     });

//     const placeholderSearchText = 'Search book by title...';
//     const fetchRef = useRef<number>(0);

//     const TABLE_HEAD: HeadLabel[] = [
//         { id: nameof<Book>('id'), label: "Id", alignRight: false, orderable: true },
//         { id: nameof<Book>('title'), label: 'Title', alignRight: false, orderable: true },
//         { id: nameof<Book>('author'), label: "Author", alignRight: false },
//         { id: nameof<Book>('description'), label: 'Description', alignRight: false },
//         { id: nameof<Book>('dateCreated'), label: 'Created At', alignRight: false, orderable: true },
//         { id: nameof<Book>('categoryId'), label: 'Category Id', alignRight: false },
//         { id: 'actions', label: 'Action' },
//     ];

//     const TABLE_ROW = (book: Book) => (
//         <>
//             <TableCell align="left">{book.id}</TableCell>
//             <TableCell align="left"><Typography variant="subtitle2" noWrap>{book.title}</Typography></TableCell>
//             <TableCell align="left">{book.author}</TableCell>
//             <TableCell align="left">{book.description}</TableCell>
//             <TableCell align="left">{book.dateCreated}</TableCell>
//             <TableCell align="left">{book.categoryId}</TableCell>
//         </>
//     );

//     const setFetchingData = (isFetching: boolean) => {
//         setListTableUseStates((prevState) => ({ ...prevState, isFetchingData: isFetching }));
//     };

//     const setFetchData = (data: FetchData<Book>) => {
//         setListTableUseStates((prevState) => ({ ...prevState, fetchData: data }));
//     };

//     const fetchBookData = async () => {
//         setFetchingData(true);
//         const { page, order, orderBy, filterName, rowsPerPage } = listTableUseStates;
//         const pagePlusOne = page + 1;
//         const params = {
//             page: pagePlusOne,
//             sortField: orderBy,
//             sortOrder: order,
//             search: filterName,
//             perPage: rowsPerPage,
//         };
//         fetchRef.current += 1;
//         const fetchId = fetchRef.current;
//         const response = await axiosInstance.get(URLConstants.BOOK.GETALL, { params });
//         if (fetchId !== fetchRef.current) return;
//         setFetchData(response.data);
//         setFetchingData(false);
//     };

//     const handleDeleteBook = async (id: string) => {
//         setFetchingData(true);
//         const response = await axiosInstance.delete(`${URLConstants.BOOK.DELETE}/${id}`);
//         return response;
//     };

//     const refreshTable = () => {
//         if (listTableUseStates.page === 0) fetchBookData();
//         else setListTableUseStates((prevState) => ({ ...prevState, page: 0 }));
//     };

//     const handleAddModalOpen = () => setListTableUseStates((prevState) => ({ ...prevState, addModalOpen: true }));
//     const handleAddModalClose = () => setListTableUseStates((prevState) => ({ ...prevState, addModalOpen: false }));
//     const handleEditModalClose = () => setListTableUseStates((prevState) => ({ ...prevState, editModalOpen: false }));

//     const debounceSetter = useMemo(() => {
//         const handleFilterByName = (event: React.ChangeEvent<HTMLInputElement>) => {
//             setListTableUseStates((previous) => ({ ...previous, page: 0, filterName: event.target.value }))
//         };
//         // eslint-disable-next-line react-hooks/exhaustive-deps
//         return debounce(handleFilterByName, 700);
//     }, []);

//     useEffect(() => {
//         fetchBookData();
//     }, [listTableUseStates.page, listTableUseStates.orderBy, listTableUseStates.order, listTableUseStates.filterName, listTableUseStates.rowsPerPage]);

//     return (
//         <>
//             <Helmet>
//                 <title> Your Borrowing Requests </title>
//             </Helmet>

//             <Container>
//                 <Stack direction="row" alignItems="center" justifyContent="space-between" mb={5}>
//                     <Typography variant="h4" gutterBottom>
//                         Your Borrowing Requests
//                     </Typography>
//                     <Button onClick={handleAddModalOpen} variant="contained" startIcon={<Iconify icon="eva:plus-fill" />}>
//                         New Borrowing Request
//                     </Button>
//                 </Stack>
//                 <Card>
//                     <StyledRoot>
//                         <StyledSearch
//                             onChange={debounceSetter}
//                             placeholder={placeholderSearchText}
//                             startAdornment={
//                                 <InputAdornment position="start">
//                                     <Iconify icon="eva:search-fill" sx={{ color: 'text.disabled', width: 20, height: 20 }} />
//                                 </InputAdornment>
//                             }
//                         />
//                     </StyledRoot>
//                     <Spin spinning={isFetchingData}>
//                         <TableContainer >
//                             <Scrollbar >
//                                 <Table>
//                                     <ListHead
//                                         order={order}
//                                         orderBy={orderBy}
//                                         headLabel={TABLE_HEAD}
//                                         rowCount={fetchDataLength}
//                                         numSelected={selected.length}
//                                         onRequestSort={handleRequestSort}
//                                         onSelectAllClick={handleSelectAllClick}
//                                     />
//                                     <TableBody>
//                                         {fetchDataData.map((row) => {
//                                             const { id } = row;
//                                             const selectedUser = selected.indexOf(id) !== -1;

//                                             return (
//                                                 <TableRow hover key={id} tabIndex={-1} role="checkbox" selected={selectedUser}>
//                                                     <TableCell padding="checkbox">
//                                                         <Checkbox checked={selectedUser} onChange={() => handleClick(id)} />
//                                                     </TableCell>

//                                                     {TABLE_ROW(row)}

//                                                     <TableCell align="right">
//                                                         <IconButton
//                                                             size="large"
//                                                             color="inherit"
//                                                             onClick={(event) => handleOpenMenu(event, id)}
//                                                         >
//                                                             <Iconify icon={'eva:more-vertical-fill'} />
//                                                         </IconButton>
//                                                     </TableCell>
//                                                 </TableRow>
//                                             );
//                                         })}
//                                     </TableBody>

//                                     {isNotFound && (
//                                         <TableBody>
//                                             <TableRow>
//                                                 <TableCell align="center" colSpan={6} sx={{ py: 3 }}>
//                                                     <Paper
//                                                         sx={{
//                                                             textAlign: 'center',
//                                                         }}
//                                                     >
//                                                         <Typography variant="h6" paragraph>
//                                                             Not found
//                                                         </Typography>

//                                                         <Typography variant="body2">
//                                                             No results found for &nbsp;<strong>&quot;{filterName}&quot;</strong>.
//                                                             <br /> Try checking for typos or using complete words.
//                                                         </Typography>
//                                                     </Paper>
//                                                 </TableCell>
//                                             </TableRow>
//                                         </TableBody>
//                                     )}
//                                 </Table>
//                             </Scrollbar>
//                         </TableContainer>
//                     </Spin>

//                     <TablePagination
//                         rowsPerPageOptions={[5, 10, 25]}
//                         component="div"
//                         count={fetchDataTotal}
//                         rowsPerPage={rowsPerPage}
//                         page={page}
//                         onPageChange={handleChangePage}
//                         onRowsPerPageChange={handleChangeRowsPerPage}
//                     />
//                 </Card>
//             </Container>
//             {/* <Modal
//                 key='add'
//                 open={listTableUseStates.addModalOpen}
//                 onClose={handleAddModalClose}
//                 aria-labelledby="modal-modal-title"
//                 aria-describedby="modal-modal-description"
//             >
//                 <StyledBox>
//                     <BookAdd fetchList={fetchBookData} />
//                 </StyledBox>
//             </Modal> */}
//         </>
//     );
// }

// export default UserBorrowingList