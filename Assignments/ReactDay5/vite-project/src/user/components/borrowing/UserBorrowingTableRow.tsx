import { Box, Collapse, IconButton, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material"
import { BorrowingRequestUser, BookStatus } from '../../pages/BorrowingPages/UserBorrowingList';
import { useState } from "react";
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';

interface UserBorrowingTableRowProps {
    request: BorrowingRequestUser
}

const UserBorrowingTableRow = ({ request }: UserBorrowingTableRowProps) => {
    const [open, setOpen] = useState(false);
    return (
        <>
            <TableRow>
                <TableCell>
                    <IconButton
                        aria-label="expand row"
                        size="small"
                        onClick={() => setOpen(!open)}
                    >
                        {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                    </IconButton>
                </TableCell>
                <TableCell align="left"><Typography variant="subtitle2" noWrap>{request.requestDate}</Typography></TableCell>
                <TableCell align="left">{BookStatus[request.status].toString()}</TableCell>
            </TableRow>

            <TableRow>
                <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={6}>
                    <Collapse in={open} timeout="auto" unmountOnExit>
                        <Box sx={{ margin: 1 }}>
                            <Typography variant="h6" gutterBottom component="div">
                                Borrowed Books
                            </Typography>
                            <Table size="small" aria-label="purchases">
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Book Title</TableCell>
                                        <TableCell>Category</TableCell>
                                        <TableCell>Borrowed Date</TableCell>
                                        <TableCell>Returned Date</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {request.bookBorrowingRequestDetails.map((details) => (
                                        <TableRow key={details.bookId}>
                                            <TableCell component="th" scope="row">
                                                {details.book.title}
                                            </TableCell>
                                            <TableCell>{details.book.category.name}</TableCell>
                                            <TableCell>{details.borrowedDate}</TableCell>
                                            <TableCell>{details.returnedDate}</TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </Box>
                    </Collapse>
                </TableCell>
            </TableRow>
        </>
    )
}

export default UserBorrowingTableRow