import { Link } from 'react-router-dom';

interface ErrorPageProps {
    code: number,
    message: string,
}

const ErrorPage = ({ code, message }: ErrorPageProps) => {
  return (
    <div>
      <h1>Error {code}</h1>
      <p>{message}</p>
      <Link to="/">Go to Home</Link>
    </div>
  );
};

const Error401 = () => {
  return <ErrorPage code={401} message="Unauthorized - You do not have permission to access this page." />;
};

const Error403 = () => {
  return <ErrorPage code={403} message="Forbidden - You do not have permission to access this page." />;
};

const Error404 = () => {
  return <ErrorPage code={404} message="Page Not Found - The page you are looking for does not exist." />;
};

const Error500 = () => {
  return <ErrorPage code={500} message="Internal Server Error - Something went wrong on the server." />;
};

export { Error401, Error403, Error404, Error500 };
