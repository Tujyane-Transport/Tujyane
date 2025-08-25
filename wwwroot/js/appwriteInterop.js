let client;
let account;

window.initAppwrite = function (endpoint, projectId) {
    client = new Appwrite.Client()
        .setEndpoint(endpoint)
        .setProject(projectId);

    account = new Appwrite.Account(client);
    console.log("Appwrite initialized ✅");
};

window.registerUser = async function (email, password, name) {
    if (!account) {
        throw new Error("Appwrite not initialized. Call initAppwrite first.");
    }
    return await account.create(Appwrite.ID.unique(), email, password, name);
};

window.loginUser = async function (email, password) {
    if (!account) {
        throw new Error("Appwrite not initialized. Call initAppwrite first.");
    }
    return await account.createEmailPasswordSession(email, password);
};

window.getCurrentUser = async function () {
    if (!account) {
        throw new Error("Appwrite not initialized. Call initAppwrite first.");
    }
    return await account.get();
};

window.logoutUser = async function () {
    if (!account) {
        throw new Error("Appwrite not initialized. Call initAppwrite first.");
    }
    try {
        await account.deleteSession("current");
        console.log("User logged out");
    } catch (err) {
        console.error("Logout failed:", err);
    }
};