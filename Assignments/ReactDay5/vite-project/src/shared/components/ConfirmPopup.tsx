import React, { useState, useCallback, ReactElement } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import Button from '@mui/material/Button';

interface ConfirmPopupProps {
    message?: string;
    children: ReactElement<{ onClick: React.MouseEventHandler }>;
}

const ConfirmPopup: React.FC<ConfirmPopupProps> = ({ message = "Are you sure to continue?", children }) => {
    const [open, setOpen] = useState(false);
    const [childClickHandler, setChildClickHandler] = useState<React.MouseEventHandler | null>(null);

    const handleClickChild = useCallback(
        (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
            event.preventDefault(); // Prevent the child component's click action
            setChildClickHandler(() => children.props.onClick);
            console.log(children.props.onClick);

            setOpen(true);
        },
        [children.props.onClick]
    );

    const handleConfirm = useCallback(() => {
        setOpen(false);
        if (childClickHandler) {
            console.log('Action confirmed');
            childClickHandler(new MouseEvent('click') as any); // Trigger the original click event
        }
    }, [childClickHandler]);

    const handleCancel = () => {
        setOpen(false);
        setChildClickHandler(null);
    };

    return (
        <>
            {React.cloneElement(children, { onClick: handleClickChild })}
            <Dialog open={open} onClose={handleCancel} aria-labelledby="confirm-dialog-title">
                <DialogTitle id="confirm-dialog-title">Confirmation</DialogTitle>
                <DialogContent>
                    <DialogContentText>{message}</DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCancel} color="primary">
                        Cancel
                    </Button>
                    <Button onClick={handleConfirm} color="primary" autoFocus>
                        Confirm
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
};

export default ConfirmPopup;
