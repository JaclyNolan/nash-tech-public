import { Backdrop, styled } from "@mui/material";

const GrayBackdrop = styled(Backdrop)(({ theme }) => ({
  zIndex: theme.zIndex.drawer + 1,
  color: '#fff',
  backgroundColor: 'rgba(128, 128, 128, 0.2)'
}));

export default GrayBackdrop;