import { Box, Checkbox, TableRow, TableCell, TableHead, TableSortLabel } from '@mui/material';
import React from 'react';

// ----------------------------------------------------------------------

const visuallyHidden: React.CSSProperties = {
  border: 0,
  margin: -1,
  padding: 0,
  width: '1px',
  height: '1px',
  overflow: 'hidden',
  position: 'absolute',
  whiteSpace: 'nowrap',
  clip: 'rect(0 0 0 0)',
};

export interface HeadLabel {
  id: string;
  label: string;
  alignRight?: boolean;
  orderable?: boolean;
  [key: string]: any;
}

interface ListHeadProps {
  order: 'asc' | 'desc';
  orderBy: string;
  rowCount: number;
  headLabel: HeadLabel[];
  numSelected: number;
  onSelectAllClick?: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onRequestSort: (event: React.MouseEvent<unknown>, property: HeadLabel) => void;
}

const ListHead: React.FC<ListHeadProps> = ({
  order,
  orderBy,
  rowCount,
  headLabel,
  numSelected,
  onSelectAllClick,
  onRequestSort,
}) => {
  const createSortHandler = (property: HeadLabel) => (event: React.MouseEvent<unknown>) => {
    onRequestSort(event, property);
  };

  return (
    <TableHead>
      <TableRow>
        {onSelectAllClick && (
          <TableCell padding="checkbox">
            <Checkbox
              indeterminate={numSelected > 0 && numSelected < rowCount}
              checked={rowCount > 0 && numSelected === rowCount}
              onChange={onSelectAllClick}
            />
          </TableCell>
        )}
        {headLabel.map((headCell) => {
          const { id, label, alignRight, orderable, ...props } = headCell;
          return (
            <TableCell
              {...props}
              key={id}
              align={alignRight ? 'right' : 'left'}
              sortDirection={orderBy === id ? order : false}
            >
              <TableSortLabel
                hideSortIcon
                active={orderable}
                direction={orderBy === id ? order : 'asc'}
                onClick={createSortHandler(headCell)}
              >
                {label}
                {orderBy === headCell.id ? (
                  <Box sx={{ ...visuallyHidden }}>
                    {order === 'desc' ? 'sorted descending' : 'sorted ascending'}
                  </Box>
                ) : null}
              </TableSortLabel>
            </TableCell>
          );
        })}
      </TableRow>
    </TableHead>
  );
};

export default ListHead;
