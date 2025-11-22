// =============================
// 📁 firebase-auth.js
// =============================

// ✅ import Firebase instance ที่ตั้งค่าไว้ใน firebase-init.js
import { auth } from './firebase-init.js';

// ✅ import ฟังก์ชัน Auth ที่ต้องใช้
import {
    createUserWithEmailAndPassword,
    signInWithEmailAndPassword,
    signOut
} from "https://www.gstatic.com/firebasejs/12.5.0/firebase-auth.js";

// ✅ ฟังก์ชัน Register พร้อม popup SweetAlert2
window.registerPopup = async () => {
    const { value: formValues } = await Swal.fire({
        title: 'สมัครสมาชิกใหม่',
        html:
            '<input id="swal-input1" class="swal2-input" placeholder="อีเมล">' +
            '<input id="swal-input2" type="password" class="swal2-input" placeholder="รหัสผ่าน">',
        focusConfirm: false,
        showCancelButton: true,
        confirmButtonText: 'สมัคร',
        cancelButtonText: 'ยกเลิก',
        preConfirm: () => {
            const email = document.getElementById('swal-input1').value;
            const password = document.getElementById('swal-input2').value;
            if (!email || !password) {
                Swal.showValidationMessage('กรุณากรอกอีเมลและรหัสผ่านให้ครบ');
                return false;
            }
            return { email, password };
        }
    });

    // ✅ ถ้าผู้ใช้กดสมัคร
    if (formValues) {
        try {
            const userCredential = await createUserWithEmailAndPassword(auth, formValues.email, formValues.password);
            Swal.fire({
                icon: 'success',
                title: 'สมัครสมาชิกสำเร็จ!',
                text: `ยินดีต้อนรับ ${userCredential.user.email}`,
                confirmButtonText: 'ตกลง'
            });
            return userCredential.user.email;
        } catch (error) {
            Swal.fire({
                icon: 'error',
                title: 'สมัครไม่สำเร็จ',
                text: error.message,
                confirmButtonText: 'ปิด'
            });
            return null;
        }
    }
};

// ✅ ฟังก์ชัน Login (แบบไม่ popup)
window.loginUser = async (email, password) => {
    try {
        const userCredential = await signInWithEmailAndPassword(auth, email, password);
        Swal.fire({
            icon: 'success',
            title: 'เข้าสู่ระบบสำเร็จ!',
            text: `ยินดีต้อนรับ ${userCredential.user.email}`,
            confirmButtonText: 'ตกลง'
        });
        return userCredential.user.email;
    } catch (error) {
        Swal.fire({
            icon: 'error',
            title: 'เข้าสู่ระบบไม่สำเร็จ',
            text: error.message,
            confirmButtonText: 'ปิด'
        });
        return null;
    }
};

// ✅ ฟังก์ชัน Logout
window.logoutUser = async () => {
    await signOut(auth);
    Swal.fire({
        icon: 'info',
        title: 'ออกจากระบบแล้ว',
        confirmButtonText: 'ตกลง'
    });
};