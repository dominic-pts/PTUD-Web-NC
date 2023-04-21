import { useEffect, useState } from 'react';
import { getTagCloud } from '../Services/Widgets';
import TagList from '../Components/TagList';

const TagCloud = () => {
  const [tagsList, setTagsList] = useState([]);

  useEffect(() => {
    getTagCloud().then((data) => {
      if (data) setTagsList(data);
      else setTagsList([]);
    });
  }, []);

  return (
    <div className="mb-4">
      <h3 className="text-success mb-2">Các thẻ tag cloud</h3>
      {tagsList.length > 0 && (
        <div className="tag-list">
          <TagList tagList={tagsList} />
        </div>
      )}
    </div>
  );
};

export default TagCloud;
