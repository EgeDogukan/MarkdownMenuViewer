/* eslint-disable react/prop-types */
import { useState, useEffect } from "react";
import "../CSS/style.css";

export default function Explore() {

    const [files, setFiles] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [selectedFileContent, setSelectedFileContent] = useState("");

    useEffect(() => {
        fetch("https://658e9ba72871a9866e7973f5.mockapi.io/markdown/v1/folder")
            .then((response) => response.json())
            .then((data) => {
                // Parse işlemi
                const parsedData = JSON.parse(JSON.stringify(data));

                // Parse edilmiş veriyi state'e ata
                setFiles(parsedData[0].files);
                setSelectedFileContent(parsedData[0].files.data.content);


                // Debug amaçlı log
                //console.log("Parse edilmiş veri:", parsedData);
                //console.log("veri:", parsedData[0].files.data[2].content);

            })
            .catch((error) => console.error("API'den veri çekme hatası:", error))
            .finally(() => setIsLoading(false));
    }, []);
   

    const handleFileClick = (content) => {
        setSelectedFileContent(content);
    };


    const [isLeftPaneOpen, setIsLeftPaneOpen] = useState(true);


    const toggleLeftPane = () => {
        setIsLeftPaneOpen(!isLeftPaneOpen);
    };




    return (
        <>
            <div className={`container-m m-0 ${isLeftPaneOpen ? '' : 'left-pane-closed'}`} id="main-container">
                {isLeftPaneOpen && (
                    <div id="file-container">
                        <div id="filess" className="d-flex justify-content">
                            <a id="panell" href="#" onClick={toggleLeftPane}>
                                {isLeftPaneOpen ? (
                                    <i id="arrow" className="bi bi-arrow-left-circle"></i>
                                ) : (
                                    <i id="arrow" className="bi bi-arrow-right-circle"></i>
                                )}
                            </a>
                            <h1 id="files-heading"> Files</h1>
                        </div>

                        <div>
                            <div id="input" className="input-group">
                                <div
                                    id="placeholder"
                                    className="form-outline d-flex justify-content"
                                    data-mdb-input-init
                                >
                                    <input
                                        id="search-input"
                                        type="search"
                                        placeholder="Search your document"
                                        className="form-control"
                                    />
                                    <button id="search-button" type="button" className="btn btn-primary">
                                        <i className="bi bi-search"></i>
                                    </button>
                                </div>
                            </div>
                        </div>

                        {isLoading ? (
                            <p>Loading...</p>
                        ) : (
                            <FileExplorer files={files} onFileClick={handleFileClick} />
                        )}
                    </div>
                )}

                <div id="content">

                    <div id="md-content-one">

                        <h2>
                            <a id="panell" href="#" onClick={toggleLeftPane}>
                                {isLeftPaneOpen ? (
                                    <i id="arrow" className="bi bi-arrow-left-circle"></i>
                                ) : (
                                    <i id="arrow" className="bi bi-arrow-right-circle"></i>
                                )}
                            </a>
                            <i className="bi bi-filetype-md"></i> MarkdownMenuViewer
                        </h2>
                    </div>

                    <div id="md-content-two">
                        {/* Dosya içeriğini göster */}
                        {selectedFileContent && (
                            <div dangerouslySetInnerHTML={{ __html: selectedFileContent }} />
                        )}
                    </div>
                </div>
            </div>
        </>
    );
}

function FileExplorer({ files, onFileClick }) {
    const [expanded, setExpanded] = useState(false);

    const toggleIcon = expanded ? "bi bi-chevron-down" : "bi bi-chevron-right";

    const handleToggle = () => {
        setExpanded(!expanded);
    };

    if (files.type === "folder") {
        return (
            <div id="con" key={files.name}>
                <span id="spann" onClick={handleToggle}>
                    <i id="arrowdown" className={toggleIcon}></i><i id="folder" className="bi bi-folder-fill"></i>&nbsp;{files.name}
                </span>
                <div className="expanded"
                    style={{ display: expanded ? "block" : "none" }}
                >
                    {files.data.map((file) => {
                        if (file.type === "file")
                            return (
                                // Dosyaya tıklandığında onFileClick fonksiyonunu çağır
                                <div
                                    id="expanded-content"
                                    key={file.name}
                                    onClick={() => onFileClick(file.content)}
                                >
                                    &nbsp;&nbsp;&nbsp;&nbsp;<i className="bi bi-file-earmark"></i>&nbsp;{file.name}
                                </div>
                            );
                        else if (file.type === "folder")
                            return (
                                <FileExplorer
                                    key={file.name}
                                    files={file}
                                    onFileClick={onFileClick}
                                />
                            );
                    })}
                </div>
            </div>
        );
    } else if (files.type === "file") {
        return <div>{files.name}</div>;
    } else {
        return <p>!!</p>;
    }
}
