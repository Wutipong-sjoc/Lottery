// =============================
// 📁 firebase-db.js
// =============================

// ✅ import database instance ที่ตั้งไว้ใน firebase-init.js
import { db } from './firebase-init.js';

// ✅ import ฟังก์ชัน Database ที่ต้องใช้
import { ref, set, get, child } from "https://www.gstatic.com/firebasejs/12.5.0/firebase-database.js";

// ✅ ฟังก์ชันบันทึกข้อมูล
window.saveToFirebase = (user, msg) => {
    set(ref(db, 'users/' + user), {
        message: msg,
        time: new Date().toISOString()
    });
};

// ✅ ฟังก์ชันอ่านข้อมูล
window.readFromFirebase = async (user) => {
    const snapshot = await get(child(ref(db), 'users/' + user));
    if (snapshot.exists()) {
        return snapshot.val();
    } else {
        return null;
    }
};