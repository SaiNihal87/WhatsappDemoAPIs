DROP TABLE IF EXISTS chat;
CREATE TABLE "chat" (
    "id" SERIAL8 PRIMARY KEY,
    "sender_id" INT8 NOT NULL REFERENCES users(id),
    "reciever_id" INT8,
    "text" VARCHAR(20) NOT NULL,
    "is_groupchat" BOOL DEFAULT FALSE,
    "group_id" INT8,
    "created_at" TIMESTAMPTZ DEFAULT NOW(),
    "updated_at" TIMESTAMPTZ

);