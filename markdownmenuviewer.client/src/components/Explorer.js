import { useState } from "react";
import "../style.css";



export default function Explore() {
  return (
    <>
      
     

      <div id="main-container">
      
      <div id="file-container" >
      <h1>My Files</h1>
        <FileExplorer files={Files} />
      </div>

      <div id="content">
       
        <h1>Content</h1>

        
      </div>


  </div>
    </>



  );
}

let Files = {
  type: "folder",
  name: "parent",
  data: [
    {
      type: "folder",
      name: "root",
      data: [
        {
          type: "folder",
          name: "src",
          data: [
            {
              type: "file",
              name: "index.js"
            }
          ]
        },
        {
          type: "folder",
          name: "public",
          data: [
            {
              type: "file",
              name: "index.ts"
            }
          ]
        },
        {
          type: "file",
          name: "index.html"
        },
        {
          type: "folder",
          name: "data",
          data: [
            {
              type: "folder",
              name: "images",
              data: [
                {
                  type: "file",
                  name: "image.jpg"
                },
                {
                  type: "file",
                  name: "image2.jpg"
                },
                {
                  type: "file",
                  name: "image3.jpg"
                }
              ]
            },
            {
              type: "file",
              name: "logo.svg"
            }
          ]
        },
        {
          type: "file",
          name: "style.css"
        }
      ]
    }
  ]
};

function FileExplorer({ files }) {
  const [expanded, setExpanded] = useState(false);
  if (files.type === "folder") {
    return (
      <div key={files.name}>
        <span id="spann" onClick={() => setExpanded(!expanded)}>{files.name}📂</span>
        <div
          className="expanded"
          style={{ display: expanded ? "block" : "none" }}
        >
          {files.data.map((file) => {
            if (file.type === "file")
              return <div id="expanded-content" key={file.name}>{file.name}</div>;
            else if (file.type === "folder")
              return <FileExplorer key={file.name} files={file} />;
          })}
        </div>
      </div>
    );
  } else if (files.type === "file") {
    return <div>{files.name}</div>;
  } else {
    return <p>⚱⚱</p>;
  }
}
