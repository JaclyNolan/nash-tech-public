import { FC } from "react";
import Navbar from "../components/navs/Navbar";
import { Outlet } from "react-router-dom";

const HomeLayout: FC = () => {
    return (
        <>
            <Navbar />
            <main style={{ padding: "1rem" }}>
                <Outlet />
            </main>
        </>
    );
}

export default HomeLayout;
