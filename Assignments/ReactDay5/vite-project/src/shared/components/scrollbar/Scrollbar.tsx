import { CSSProperties, ReactNode, memo } from 'react';
import { Box, BoxProps } from '@mui/material';
import { StyledRootScrollbar, StyledScrollbar } from './styles';

interface ScrollbarProps {
  sx?: CSSProperties;
  children?: ReactNode;
  timeout?: number;
  clickOnTrack?: boolean;
}

function Scrollbar({ children, sx, clickOnTrack = false, ...other }: ScrollbarProps & BoxProps) {
  const userAgent = typeof navigator === 'undefined' ? 'SSR' : navigator.userAgent;

  const isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(userAgent);

  if (isMobile) {
    return (
      <Box sx={{ overflowX: 'auto', ...sx }} {...other}>
        {children}
      </Box>
    );
  }

  return (
    <StyledRootScrollbar>
      <StyledScrollbar clickOnTrack={clickOnTrack} sx={sx} {...other}>
        {children}
      </StyledScrollbar>
    </StyledRootScrollbar>
  );
}

export default memo(Scrollbar);
