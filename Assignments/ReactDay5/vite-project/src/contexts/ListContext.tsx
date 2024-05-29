import React, { createContext, useState, ReactNode, useContext } from 'react';


interface FetchData {
  current_page: number;
  data: Topic[];
  first_page_url: string;
  from: number;
  last_page: number;
  last_page_url: string;
  links: any[];
  next_page_url: string | null;
  path: string;
  per_page: number;
  prev_page_url: string | null;
  to: number;
  total: number;
}

interface ListContextProps {
  openEntry: Topic | null;
  setOpenEntry: React.Dispatch<React.SetStateAction<Topic | null>>;
  page: number;
  setPage: React.Dispatch<React.SetStateAction<number>>;
  order: 'asc' | 'desc';
  setOrder: React.Dispatch<React.SetStateAction<'asc' | 'desc'>>;
  selected: number[];
  setSelected: React.Dispatch<React.SetStateAction<number[]>>;
  orderBy: string;
  setOrderBy: React.Dispatch<React.SetStateAction<string>>;
  filterName: string;
  setFilterName: React.Dispatch<React.SetStateAction<string>>;
  rowsPerPage: number;
  setRowsPerPage: React.Dispatch<React.SetStateAction<number>>;
  fetchData: FetchData | null;
  setFetchData: React.Dispatch<React.SetStateAction<FetchData | null>>;
  isFetchingData: boolean;
  setFetchingData: React.Dispatch<React.SetStateAction<boolean>>;
}

const ListContext = createContext<ListContextProps | undefined>(undefined);

export const ListProvider = ({ children }: { children: ReactNode }) => {
  const [openEntry, setOpenEntry] = useState<Topic | null>(null);
  const [page, setPage] = useState<number>(0);
  const [order, setOrder] = useState<'asc' | 'desc'>('asc');
  const [selected, setSelected] = useState<number[]>([]);
  const [orderBy, setOrderBy] = useState<string>('name');
  const [filterName, setFilterName] = useState<string>('');
  const [rowsPerPage, setRowsPerPage] = useState<number>(5);
  const [fetchData, setFetchData] = useState<FetchData | null>(null);
  const [isFetchingData, setFetchingData] = useState<boolean>(true);

  return (
    <ListContext.Provider
      value={{
        openEntry,
        setOpenEntry,
        page,
        setPage,
        order,
        setOrder,
        selected,
        setSelected,
        orderBy,
     
