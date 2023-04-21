import React from "react";
import SearchFrorm from "./SearchFrorm";
import CategoriesWidget from "./CategoriesWidget";
import FeaturedPosts from "./FeaturedPosts";
import RandomPostsWidget from "./RandomPostsWidget"
import TagCloud from "./TagCloud"
import BestAuthors from "./BestAuthors"
import NewsLetterForm from "./NewsLetterFrom";

export default function Sidebar() {
  return (
    <div className="pt-4 ps-2">
      
      <SearchFrorm />

      <CategoriesWidget />

      <FeaturedPosts/>

      <RandomPostsWidget/>

      <TagCloud/>
      
      <NewsLetterForm/>

      <BestAuthors/>

 
    </div>
  );
}
