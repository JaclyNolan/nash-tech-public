import React from 'react';
import Chip from '@mui/material/Chip';
import { BookStatus } from '../../user/pages/BorrowingPages/UserBorrowingList';

interface BookStatusComponentProps {
    status: BookStatus;
}

const BookStatusComponent: React.FC<BookStatusComponentProps> = ({ status }) => {
    // Define a function to get the color based on the status
    const getStatusColor = (status: BookStatus) => {
        switch (status) {
            case BookStatus.Waiting:
                return 'primary';
            case BookStatus.Approved:
                return 'success';
            case BookStatus.Rejected:
                return 'error';
            default:
                return 'default'; // or any other default color
        }
    };

    return (
        <Chip label={BookStatus[status]} color={getStatusColor(status)} />
    );
};

export default BookStatusComponent;
