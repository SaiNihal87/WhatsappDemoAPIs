DROP TABLE IF EXISTS "calls";
CREATE TABLE "calls" (
    "id" SERIAL8 PRIMARY KEY,
    "caller_id" INT8 NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    "reciever_id" INT8 NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    "created_at" TIMESTAMPTZ DEFAULT NOW()
);