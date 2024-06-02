import { LoadingButton } from '@mui/lab';
import { Grid, Stack } from '@mui/material';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import * as React from 'react';
import { useState } from 'react';
import axiosInstance from '../../../axiosInstance';
import { URLConstants } from '../../../common/constants';
import DebounceSelect, { SelectOption } from '../../../shared/components/table/DebounceSelect';

interface BookAddProps {
  fetchList: () => void;
}

interface Category {
  id: number;
  name: string;
}

export default function BookAdd({ fetchList }: Readonly<BookAddProps>) {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [bookTitle, setBookTitle] = useState('');
  const [author, setAuthor] = useState('');
  const [category, setCategory] = useState<SelectOption | null>(null);
  const [bookDescription, setBookDescription] = useState('');

  const fetchCategoryData = async (search: string): Promise<SelectOption[]> => {
    const params = { search };
    const response = await axiosInstance.get(URLConstants.CATEGORY.GETALL, { params });
    const categories: Category[] = response.data.categories;
    return categories.map((category) => ({
      value: category.id,
      label: `${category.id} ${category.name}`,
    }));
  };

  const handleFormSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setIsSubmitting(true);
    const payload = {
      title: bookTitle,
      author,
      description: bookDescription,
      category_id: category?.value,
    };
    try {
      const response = await axiosInstance.post(URLConstants.BOOK.ADD, payload);
      const book = response.data;
      alert(`Added successfully! Id: ${book.id}, Title: ${book.title}`);
      fetchList();
    } catch (error) {
      console.error('Error adding book:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Stack spacing={3}>
      <Typography variant="h6" gutterBottom>
        Add Book
      </Typography>
      <form onSubmit={handleFormSubmit}>
        <Grid container spacing={3}>
          <Grid item xs={12} sm={6}>
            <TextField
              required
              fullWidth
              id='book_title'
              name='book_title'
              label='Book Title'
              value={bookTitle}
              onChange={(event) => setBookTitle(event.target.value)}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              required
              fullWidth
              id='author'
              name='author'
              label='Author'
              value={author}
              onChange={(event) => setAuthor(event.target.value)}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <DebounceSelect
              required
              id='category'
              name='category'
              label="Category"
              placeholder="Select Category"
              value={category}
              onChange={setCategory}
              fetchOptions={fetchCategoryData}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              id='book_description'
              name='book_description'
              label='Description'
              value={bookDescription}
              onChange={(event) => setBookDescription(event.target.value)}
              fullWidth
              multiline
              rows={4}
              placeholder='Description'
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <LoadingButton loading={isSubmitting} fullWidth size="large" type="submit" variant="contained">
              Add
            </LoadingButton>
          </Grid>
        </Grid>
      </form>
    </Stack>
  );
}
