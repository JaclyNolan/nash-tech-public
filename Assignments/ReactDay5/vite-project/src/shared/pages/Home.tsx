import { FC } from "react";
import { Helmet } from "react-helmet-async";

const Home: FC = () => {
    return (
        <>
            <Helmet>
                <title>Home</title>
            </Helmet>
            <div>This is Home</div>
        </>
    )
}

export default Home;