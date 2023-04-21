import React from 'react'

import Navbar from "../../Components/Admin/NavBar";
import { Outlet } from 'react-router-dom'

const AdminLayout = () => {
  return (
    <>
      <Navbar />
      <div className="container-fluid py-3">
        <Outlet />
      </div>
    </>
  );
};
export default AdminLayout;