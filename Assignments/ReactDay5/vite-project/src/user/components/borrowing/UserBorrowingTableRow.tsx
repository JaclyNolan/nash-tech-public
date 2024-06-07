import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import { Box, Collapse, IconButton, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material";
import { useState } from "react";
import { toStandardFormat } from "../../../common/helper";
import BookStatusComponent from '../../../shared/components/BookStatusComponent';
import { BorrowingRequestUser } from '../../pages/BorrowingPages/UserBorrowingList';

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
                <TableCell align="left"><Typography variant="subtitle2" noWrap>{toStandardFormat(request.requestDate)}</Typography></TableCell>
                <TableCell align="left"><BookStatusComponent status={request.status}/></TableCell>
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
                                            <TableCell>{toStandardFormat(details.borrowedDate)}</TableCell>
                                            <TableCell>{toStandardFormat(details.returnedDate)}</TableCell>
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